using System.Net;
using System.Net.Mail;
using bra_reint_API.Models.ViewModel;

namespace bra_reint_API.Services.EmailServices;

public class EmailSender : IEmailSender
{
    private const string SmtpEmail = "no-reply@brareint.no";
    private const string AdminEmail = "roy-eo@hotmail.com"; // "post@brareint.no";
    private const string SmtpHost = "smtp.domeneshop.no";
    private const int SmtpPort = 587;

    public async Task SendEmail(CreateBookingViewModel booking, List<string> bookingTypes, string city,
        decimal totalPrice)
    {
        var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        if (string.IsNullOrEmpty(smtpPassword))
        {
            throw new InvalidOperationException("SMTP password is not configured.");
        }

        using var client = new SmtpClient(SmtpHost, SmtpPort);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(SmtpEmail, smtpPassword);

        // Customer email
        var customerEmail = new MailMessage
        {
            From = new MailAddress(SmtpEmail),
            Subject = "Ordrebekreftelse - Bra Reint",
            IsBodyHtml = true,
            Body = BuildCustomerEmailBody(booking, bookingTypes, city, totalPrice)
        };
        customerEmail.To.Add(booking.Email);
        await client.SendMailAsync(customerEmail);

        // Admin notification email
        var adminEmail = new MailMessage
        {
            From = new MailAddress(SmtpEmail),
            Subject = $"Ny bestilling: {booking.FirstName} {booking.LastName}",
            IsBodyHtml = true,
            Body = BuildAdminEmailBody(booking, bookingTypes, city, totalPrice)
        };
        adminEmail.To.Add(AdminEmail);
        await client.SendMailAsync(adminEmail);
    }

    private static string BuildCustomerEmailBody(CreateBookingViewModel booking, List<string> bookingTypes, string city, decimal totalPrice)
    {
        var servicesHtml = string.Join("", bookingTypes.Select(type => $"<li>{type}</li>"));

        return $"""
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #2C3E50;'>Takk for din bestilling, {booking.FirstName}!</h2>
                    <p>Her er detaljene for din bestilling:</p>
                    <ul>
                        <li><strong>Tjenester:</strong>
                            <ul>
                                {servicesHtml}
                            </ul>
                        </li>
                        <li><strong>Totalpris:</strong> NOK {totalPrice}</li>
                        <li><strong>Startdato:</strong> {booking.StartDate:yyyy-MM-dd}</li>
                        <li><strong>Adresse:</strong> {booking.Street}, {city}, {booking.PostalCode}</li>
                    </ul>
                    <p>Vi vil kontakte deg med mer informasjon.</p>
                    <p>Med vennlig hilsen,</p>
                    <p><strong>Bra Reint</strong></p>
                    <p style='font-size: 0.9em; color: #888; margin-top: 30px;'><em>
                    PS: Dette er en automatisk e-post sendt fra en no-reply-adresse. Vennligst ikke svar på denne meldingen.</em></p>
                </div>
                """;
    }

    private static string BuildAdminEmailBody(CreateBookingViewModel booking, List<string> bookingTypes, string city, decimal totalPrice)
    {
        var servicesHtml = string.Join("", bookingTypes.Select(type => $"<li>{type}</li>"));

        return $"""
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #E74C3C;'>Ny bestilling mottatt</h2>
                    <p><strong>Navn:</strong> {booking.FirstName} {booking.LastName}</p>
                    <p><strong>E-post:</strong> {booking.Email}</p>
                    <p><strong>Telefonnummer:</strong> {booking.PhoneNumber}</p>
                    <p><strong>Tjenester:</strong></p>
                    <ul>
                        {servicesHtml}
                    </ul>
                    <p><strong>Totalpris:</strong> NOK {totalPrice}</p>
                    <p><strong>Startdato:</strong> {booking.StartDate:yyyy-MM-dd}</p>
                    <p><strong>Adresse:</strong> {booking.Street}, {city}, {booking.PostalCode}</p>
                    <hr>
                    <p>Sjekk adminpanelet for mer informasjon.</p>
                </div>
                """;
    }
}


/*
{
    "email": "roy-eo@hotmail.com",
    "phoneNumber": "41678985"
    "firstName": "Roy Espen",
    "lastName": "Olsen",
    "street": "Bjørkveien 21c",
    "postalCode": "8800",
    "typeIds": [
    3,2
    ],
    "startDate": "2025-04-09T16:41:59.953Z"
}
*/