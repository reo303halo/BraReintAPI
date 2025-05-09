using bra_reint_API.Models.ViewModel;
using bra_reint_API.Services.AuthServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bra_reint_API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, UserManager<IdentityUser> userManager)
    : ControllerBase
{
    /*
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser(LoginDto user)
    {
        var identityUser = await userManager.FindByNameAsync(user.Username);
        if (identityUser != null)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "User already exists. Please log in instead..."
            });
        }

        if (await authService.RegisterUser(user))
        {
            return Ok(new
            {
                IsSuccess = true,
                Message = "User successfully registered!"
            });
        }
        
        return BadRequest(new
        {
            IsSuccess = false,
            Message = "Something went wrong..."
        });
    }
    */
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto user)
    {
        var identityUser = await userManager.FindByNameAsync(user.Username);
        if (identityUser == null)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "User does not exist..."
            });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "Invalid ModelState..."
            });
        }
        
        if (!await authService.Login(user)) 
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "Something went wrong when logging in..."
            });
        }
    
        return Ok(new
        {
            IsSuccess = true,
            Token = authService.GenerateTokenString(identityUser),
            Message = "Login Successful!"
        });
    }
}