using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bra_reint_API.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace bra_reint_API.Services.AuthServices;

public class AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
    : IAuthService
{
    public async Task<bool> RegisterUser(LoginDto user)
    {
        var identityUser = new IdentityUser
        {
            UserName = user.Username,
            Email = user.Username
        };

        var result = await userManager.CreateAsync(identityUser, user.Password);
        return result.Succeeded;
    }

    public async Task<bool> Login(LoginDto user)
    {
        var identityUser = await userManager.FindByEmailAsync(user.Username);
        if (identityUser is null)
        {
            return false;
        }

        return await userManager.CheckPasswordAsync(identityUser, user.Password);
    }

    public string GenerateTokenString(IdentityUser user)
    {
        if (user is not { UserName: not null, Email: not null }) return "Username or email missing.";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var jwtKey = config.GetSection("Jwt:Key").Value;
        if (jwtKey != null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(48),
                issuer: config.GetSection("Jwt:Issuer").Value,
                audience: config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials
            );
        
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine("Token: " + tokenString);
            return tokenString;
        }

        return "JwtKey missing.";
    }
}