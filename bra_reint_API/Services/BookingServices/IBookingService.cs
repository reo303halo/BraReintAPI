using bra_reint_API.Models;

namespace bra_reint_API.Services.BookingServices;

public interface IBookingService
{
    Task<List<Booking>> GetBookingsAsync();
    Task<List<DateTime>> GetAvailableDatesAsync();
    Task<(bool Success, string Message)> CreateBookingAsync(Booking booking, List<int> bookingTypeIds);
    Task<BookingType?> GetBookingTypeAsync(int typeId);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<Customer> CreateCustomerAsync(Customer customer);
    
    // New for Special Availability Dates
    Task<List<SpecialAvailabilityDate>> GetSpecialAvailabilityDatesAsync();
    Task<SpecialAvailabilityDate> AddOrUpdateSpecialAvailabilityDateAsync(SpecialAvailabilityDate specialDate);
}