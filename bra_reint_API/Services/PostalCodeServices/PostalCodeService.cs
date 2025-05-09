using bra_reint_API.Data;
using bra_reint_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bra_reint_API.Services.PostalCodeServices;

public class PostalCodeService(ApplicationDbContext context) : IPostalCodeService
{
    public async Task<List<PostalCode>> GetPostalCodesAsync()
    {
        return await context.PostalCodes
            .ToListAsync();
    }

    public async Task<PostalCode?> GetPostalCodeAsync(string postalCode)
    {
        return await context.PostalCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Code == postalCode);
    }

    public async Task Save(PostalCode postalCode)
    {
        var existingPostal = await context.PostalCodes.FindAsync(postalCode.Code);

        if (existingPostal == null)
        {
            context.PostalCodes.Add(postalCode);
        }
        else
        {
            existingPostal.City = postalCode.City;
            // no need to call Update() or set states manually
        }

        await context.SaveChangesAsync();
    }


    public async Task Delete(string postalCodeCode)
    {
        var postalCode = await context.PostalCodes.FindAsync(postalCodeCode);

        context.PostalCodes.Remove(postalCode ??
                                   throw new InvalidOperationException(
                                       $"No postal code with code {postalCodeCode} found."));
        await context.SaveChangesAsync();
    }
}