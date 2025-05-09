using bra_reint_API.Models;
using bra_reint_API.Models.ViewModel;

namespace bra_reint_API.Services.BookingTypeServices;

public interface IBookingTypeService
{
    Task<List<BookingType>> GetBookingTypesAsync();

    Task<BookingType?> GetBookingType(int id);
    
    Task<BookingType?> GetBookingType(string typeName);

    Task Save(BookingType newBookingType);

    Task Delete(int id);
}