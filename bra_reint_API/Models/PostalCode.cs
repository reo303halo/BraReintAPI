using System.ComponentModel.DataAnnotations;

namespace bra_reint_API.Models;

public class PostalCode
{
    [Key]
    [MaxLength(4)]
    public string Code { get; set; } = string.Empty;
    [Required]
    [MaxLength(200)]
    public string City { get; set; } = string.Empty;
    public ICollection<Address>? Addresses { get; set; }
}