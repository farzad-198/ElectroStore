using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;


namespace ElectroStore.Services
{

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var host = _configuration["Smtp:Host"] ?? throw new InvalidOperationException("SMTP host configuration is missing.");
            var portString = _configuration["Smtp:Port"];
            if (string.IsNullOrEmpty(portString))
            {
                throw new InvalidOperationException("SMTP port configuration is missing.");
            }
            var port = int.Parse(portString);
            var username = _configuration["Smtp:Username"] ?? throw new InvalidOperationException("SMTP username configuration is missing.");
            var password = _configuration["Smtp:Password"] ?? throw new InvalidOperationException("SMTP password configuration is missing.");
            var from = _configuration["Smtp:From"] ?? throw new InvalidOperationException("SMTP from address configuration is missing.");
            using var Smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };
            using var mail = new MailMessage
            {
                From = new MailAddress(from, "EtectroStore Suport"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mail.To.Add(to);
            await Smtp.SendMailAsync(mail);
        }

     


    }
}
