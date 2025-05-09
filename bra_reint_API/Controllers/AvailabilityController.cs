using bra_reint_API.Models;
using bra_reint_API.Services.BookingServices;
using Microsoft.AspNetCore.Mvc;

namespace bra_reint_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailabilityController(IBookingService bookingService) : ControllerBase
{
    // Constructor injection for IBookingService (interface)

    [HttpGet("available-dates")]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableDates()
    {
        var availableDates = await bookingService.GetAvailableDatesAsync();
        return Ok(availableDates);
    }
    
    [HttpGet("special-dates")]
    public async Task<ActionResult<List<SpecialAvailabilityDate>>> GetSpecialAvailabilityDates()
    {
        var dates = await bookingService.GetSpecialAvailabilityDatesAsync();
        return Ok(dates);
    }

    [HttpPost("special-date")]
    public async Task<ActionResult<SpecialAvailabilityDate>> AddOrUpdateSpecialDate([FromBody] SpecialAvailabilityDate specialDate)
    {
        if (specialDate.Date == default)
            return BadRequest("Date is required.");

        var result = await bookingService.AddOrUpdateSpecialAvailabilityDateAsync(specialDate);
        return Ok(result);
    }
}