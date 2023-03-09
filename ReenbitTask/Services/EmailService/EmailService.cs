using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ReenbitTask.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmail(string email)
        {
            // Getting sender info from configuration.
            var Using = _config["Email:Using"];
            var SenderEmail = _config[$"Email:{Using}:SenderEmail"];
            var SenderPassword = _config[$"Email:{Using}:SenderPassword"];
            var Host = _config[$"Email:{Using}:Host"];
            var Port = Convert.ToInt32(_config[$"Email:{Using}:Port"]);

            // Building Email.
            var Email = new MimeMessage();
            Email.From.Add(MailboxAddress.Parse(SenderEmail));
            Email.To.Add(MailboxAddress.Parse(email));
            Email.Body = new TextPart { Text = "File is successfully uploaded" };

            // Sending Email.
            try
            {
                var smtp = new SmtpClient();
                smtp.Connect(Host, Port);
                smtp.Authenticate(SenderEmail, SenderPassword);
                await smtp.SendAsync(Email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
