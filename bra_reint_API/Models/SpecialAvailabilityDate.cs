namespace bra_reint_API.Models;

public class SpecialAvailabilityDate
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; } // true = add, false = remove
}