namespace bra_reint_API.Models.ViewModel;

public class CreateBookingTypeViewModel
{
    public string TypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}