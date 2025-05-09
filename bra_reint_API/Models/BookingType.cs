using System.ComponentModel.DataAnnotations;

namespace bra_reint_API.Models;

public class BookingType
{
    public int Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string TypeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(254)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }


    public List<BookingToBookingType> BookingToBookingTypes { get; set; } = [];
}