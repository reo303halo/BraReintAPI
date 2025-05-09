using bra_reint_API.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace bra_reint_API.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterUser(LoginDto user);
    
    Task<bool> Login(LoginDto user);
    
    string GenerateTokenString(IdentityUser user);
}