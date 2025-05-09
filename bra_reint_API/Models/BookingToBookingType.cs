namespace bra_reint_API.Models;

public class BookingToBookingType
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public int BookingTypeId { get; set; }
    public BookingType BookingType { get; set; } = null!;
}