using System.ComponentModel.DataAnnotations;

namespace bra_reint_API.Models.ViewModel;

public class LoginDto
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}