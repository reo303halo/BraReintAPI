using bra_reint_API.Data;
using bra_reint_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bra_reint_API.Services.BookingTypeServices;

public class BookingTypeService(ApplicationDbContext context) : IBookingTypeService
{
    public async Task<List<BookingType>> GetBookingTypesAsync()
    {
        return await context.BookingTypes.ToListAsync();
    }

    public async Task<BookingType?> GetBookingType(int id)
    {
        return await context.BookingTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<BookingType?> GetBookingType(string typeName)
    {
        return await context.BookingTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TypeName == typeName);
    }
    
    public async Task Save(BookingType newBookingType)
    {
        var existingBookingType = await context.BookingTypes
            .FirstOrDefaultAsync(bt => bt.TypeName == newBookingType.TypeName);
        
        if (existingBookingType == null)
        {
            context.BookingTypes.Add(newBookingType);
        }
        else
        {
            // For edit
            existingBookingType.Description = newBookingType.Description;
            existingBookingType.Price = newBookingType.Price;
        }
        
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var bookingType = await context.BookingTypes.FindAsync(id);
        
        context.BookingTypes.Remove(bookingType ?? 
                                    throw new InvalidOperationException(
                                        $"No BookingType with id {id} found."));
        await context.SaveChangesAsync();
    }
}