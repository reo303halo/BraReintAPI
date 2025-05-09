using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bra_reint_API.Models;

public class Address
{
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Street { get; set; } = string.Empty;
    [Required]
    [MaxLength(4)]
    [ForeignKey("PostalCodeId")]
    public string PostalCodeId { get; set; } = string.Empty;
    public PostalCode? PostalCode { get; set; }
}
