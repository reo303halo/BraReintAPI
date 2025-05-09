using bra_reint_API.Models;
using bra_reint_API.Models.ViewModel;
using bra_reint_API.Services.BookingServices;
using bra_reint_API.Services.EmailServices;
using bra_reint_API.Services.PostalCodeServices;
using Microsoft.AspNetCore.Mvc;

namespace bra_reint_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(
    IBookingService bookingService, 
    IPostalCodeService postalCodeService, 
    IEmailSender emailSender
    ) : ControllerBase
{
    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookingViewModel>>> GetBookings()
    {
        var bookings = await bookingService.GetBookingsAsync();

        var bookingViewModels = bookings
            .Where(b => b.Customer != null) // Only include bookings with a valid customer
            .Select(b => new GetBookingViewModel
            {
                Id = b.Id,
                CustomerEmail = b.Customer!.Email,
                CustomerPhoneNumber = b.Customer!.PhoneNumber,
                CustomerFirstName = b.Customer.Firstname,
                CustomerLastName = b.Customer.Lastname,
                StartDate = b.StartDate,
                BookingTypes = [.. b.BookingBookingTypes.Select(bbt => new GetBookingTypeViewModel
                {
                    Id = bbt.BookingType.Id,
                    TypeName = bbt.BookingType.TypeName,
                    Description = bbt.BookingType.Description,
                    Price = bbt.BookingType.Price
                })]
            }).ToList();

        return Ok(bookingViewModels);
    }


    // POST: api/Bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var postalCode = await postalCodeService.GetPostalCodeAsync(model.PostalCode);
        if (postalCode == null)
        {
            return BadRequest("Invalid postal code.");
        }

        var customer = await bookingService.GetCustomerByEmailAsync(model.Email);
        if (customer == null)
        {
            var address = new Address
            {
                Street = model.Street,
                PostalCodeId = postalCode.Code
            };

            var newCustomer = new Customer
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Address = address
            };

            customer = await bookingService.CreateCustomerAsync(newCustomer);
        }
        
        List<string> bookingTypes = [];
        decimal totalPrice = 0;
        foreach (var id in model.TypeIds)
        {
            var bookingType = await bookingService.GetBookingTypeAsync(id);
            if (bookingType == null)
            {
                return BadRequest($"Invalid booking type ID: {id}");
            }
            bookingTypes.Add(bookingType.TypeName);
            totalPrice += bookingType.Price;
        }

        var booking = new Booking
        {
            CustomerId = customer.Id,
            StartDate = model.StartDate
        };

        var result = await bookingService.CreateBookingAsync(booking, model.TypeIds);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        
        // Send confirmation email
        await emailSender.SendEmail(model, bookingTypes, postalCode.City, totalPrice);

        return Ok("Booking created successfully.");
    }
}

