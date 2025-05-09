namespace bra_reint_API.Models.ViewModel;

public class GetBookingViewModel
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhoneNumber { get; set; } = string.Empty;
    public string CustomerFirstName { get; set; } = string.Empty;
    public string CustomerLastName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public List<GetBookingTypeViewModel> BookingTypes { get; set; } = [];
}