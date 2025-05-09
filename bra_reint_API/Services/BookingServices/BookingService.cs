using bra_reint_API.Data;
using bra_reint_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bra_reint_API.Services.BookingServices;

public class BookingService(ApplicationDbContext context) : IBookingService
{
    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.BookingBookingTypes)
                .ThenInclude(bbt => bbt.BookingType)
            .ToListAsync();
    }

    // Fetch available dates (Monday–Friday, next 2 months)
    public async Task<List<DateTime>> GetAvailableDatesAsync()
    {
        var today = DateTime.Today;
        var nextMonth = today.AddMonths(2);

        var bookedDates = await context.Bookings
            .Select(b => b.StartDate.Date)
            .ToListAsync();

        var baseAvailableDates = Enumerable.Range(0, (nextMonth - today).Days)
            .Select(offset => today.AddDays(offset))
            .Where(date => date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday)
            .Where(date => !bookedDates.Contains(date.Date))
            .ToHashSet();

        var specialDates = await context.SpecialAvailabilityDates.ToListAsync();

        foreach (var special in specialDates)
        {
            if (special.IsAvailable)
                baseAvailableDates.Add(special.Date.Date);   // Add override
            else
                baseAvailableDates.Remove(special.Date.Date); // Remove override
        }

        return baseAvailableDates.Order().ToList();
    }
    
    public async Task<List<SpecialAvailabilityDate>> GetSpecialAvailabilityDatesAsync()
    {
        return await context.SpecialAvailabilityDates
            .OrderBy(d => d.Date)
            .ToListAsync();
    }

    public async Task<SpecialAvailabilityDate> AddOrUpdateSpecialAvailabilityDateAsync(SpecialAvailabilityDate specialDate)
    {
        var existing = await context.SpecialAvailabilityDates
            .FirstOrDefaultAsync(d => d.Date.Date == specialDate.Date.Date);

        if (existing != null)
        {
            existing.IsAvailable = specialDate.IsAvailable;
        }
        else
        {
            context.SpecialAvailabilityDates.Add(specialDate);
        }

        await context.SaveChangesAsync();
        return specialDate;
    }
    
    public async Task<(bool Success, string Message)> CreateBookingAsync(Booking booking, List<int> bookingTypeIds)
    {
        var isDateTaken = await context.Bookings
            .AnyAsync(b => b.StartDate.Date == booking.StartDate.Date);

        if (isDateTaken)
            return (false, "This date is already booked.");

        booking.BookingBookingTypes = bookingTypeIds
            .Select(id => new BookingToBookingType { BookingTypeId = id })
            .ToList();

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return (true, "Booking created successfully.");
    }
    
    public async Task<BookingType?> GetBookingTypeAsync(int typeId)
    {
        return await context.BookingTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(bt => bt.Id == typeId);
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return await context.Customers
            .AsNoTracking()
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }
    
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        customer.Email = customer.Email.ToLower();
        
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
        return customer;
    }
}