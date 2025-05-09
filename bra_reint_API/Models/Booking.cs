using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bra_reint_API.Models;

public class Booking
{
    public int Id { get; set; }
    [Required]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public List<BookingToBookingType> BookingBookingTypes { get; set; } = [];
}

    // [Required]
    // public DateTime EndDate { get; set; }
    
     // public string Status { get; set; } = "Pending";