using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace BoxBuildproj.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var senderName = emailSettings["SenderName"];
            var senderEmail = emailSettings["SenderEmail"];
            var smtpServer = emailSettings["SmtpServer"];
            var password = emailSettings["Password"];

            // Convert port and SSL settings safely
            if (!int.TryParse(emailSettings["Port"], out int port))
                throw new InvalidOperationException("Invalid SMTP port configuration.");

            if (!bool.TryParse(emailSettings["EnableSSL"], out bool enableSSL))
                throw new InvalidOperationException("Invalid EnableSSL configuration.");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using var smtp = new SmtpClient();
            try
            {
                // Correct handling of SecureSocketOptions
                SecureSocketOptions socketOptions = (port == 587) ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;

                await smtp.ConnectAsync(smtpServer, port, socketOptions);
                await smtp.AuthenticateAsync(senderEmail, password);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Email sending failed: {ex.Message}");
            }
        }

        public async Task SendConfirmationEmailAsync(string userId, string code, string baseUrl)
        {
            // Manually build the confirmation link
            var confirmationLink = $"{baseUrl}/Account/ConfirmEmail?userId={userId}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

            var emailBody = $"Please confirm your email by clicking the following link: <a href='{confirmationLink}'>Confirm Email</a>";

            // Call SendEmailAsync to send the email with the confirmation link
            await SendEmailAsync(userId, "Confirm Your Email", emailBody);
        }
    }
}
