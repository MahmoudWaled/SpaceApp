using Microsoft.Extensions.Configuration;
using Space.Application.Interfaces.Email;
using System.Net;
using System.Net.Mail;

namespace Space.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentNullException(nameof(toEmail), "Recipient email cannot be empty.");
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject), "Subject cannot be empty.");
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException(nameof(body), "Email body cannot be empty.");

            try
            {
                var smtpClient = new SmtpClient(configuration["Smtp:Host"])
                {
                    Port = int.Parse(configuration["Smtp:Port"]),
                    Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"]),
                    EnableSsl = bool.Parse(configuration["Smtp:EnableSsl"])
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(configuration["Smtp:FromEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException($"Failed to send email to {toEmail}: SMTP error - {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send email to {toEmail}: {ex.Message}", ex);
            }
        }
    }
}