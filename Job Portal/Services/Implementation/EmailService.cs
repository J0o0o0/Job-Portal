using Job_Portal.Services.Interfaces;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace Job_Portal.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body, string attachmentPath = null)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            var builder = new BodyBuilder { HtmlBody = body };

            if (!string.IsNullOrEmpty(attachmentPath))
            {
                builder.Attachments.Add(attachmentPath);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
           // smtp.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

            smtp.Authenticate(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
