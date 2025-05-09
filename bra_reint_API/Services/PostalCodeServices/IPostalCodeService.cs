using bra_reint_API.Models;

namespace bra_reint_API.Services.PostalCodeServices;

public interface IPostalCodeService
{
    Task<List<PostalCode>> GetPostalCodesAsync();
    
    Task<PostalCode?> GetPostalCodeAsync(string postalCode);

    Task Save(PostalCode postalCode);

    Task Delete(string postalCode);
}