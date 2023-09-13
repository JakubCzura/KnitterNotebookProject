using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class EmailService : IEmailService
    {
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public async Task SendEmailAsync(SendEmailDto sendEmailDto)
        {
            //Important!
            //It is not safe to store password in appsettings.json file
            //I used it only for learning purposes and didn't send credentials to GitHub
            //Email and password are not stored in appsettings.json so sending emails will not work when you clone this repository
            var emailAddress = _configuration.GetValue<string>("EmailSending:Email");
            var password = _configuration.GetValue<string>("EmailSending:Password");
            var senderName = _configuration.GetValue<string>("EmailSending:SenderName");

            MimeMessage email = new()
            {
                Sender = new MailboxAddress(senderName, emailAddress)
            };
            email.To.Add(MailboxAddress.Parse(sendEmailDto.To));
            email.Subject = sendEmailDto.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = sendEmailDto.Body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}