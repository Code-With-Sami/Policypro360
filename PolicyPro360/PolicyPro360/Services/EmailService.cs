using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PolicyPro360.Models;

namespace PolicyPro360.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        {
            var message = new MimeMessage();
            if (!string.IsNullOrWhiteSpace(_settings.DisplayName))
            {
                message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            }
            else
            {
                message.From.Add(MailboxAddress.Parse(_settings.From));
            }
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, ct);
            if (!string.IsNullOrWhiteSpace(_settings.Username))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            }
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}


