using bra_reint_API.Models.ViewModel;

namespace bra_reint_API.Services.EmailServices;

public interface IEmailSender
{
    public Task SendEmail(CreateBookingViewModel booking, List<string> bookingTypes, string city, decimal totalPrice);
}