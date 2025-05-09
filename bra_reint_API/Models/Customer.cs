using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bra_reint_API.Models;

public class Customer
{
    public int Id { get; set; }
    [Required] 
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;
    [Phone] 
    [Required]
    [MaxLength(20)] 
    public string PhoneNumber { get; set; } = string.Empty;
    [Required] 
    [MaxLength(254)]
    public string Firstname { get; set; } = string.Empty;
    [MaxLength(254)]
    public string Lastname { get; set; } = string.Empty;
    [Required]
    [ForeignKey("Address")]
    public int AddressId { get; set; }
    public Address? Address { get; set; }
}