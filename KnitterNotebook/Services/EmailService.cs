using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(SendEmailDto sendEmailDto)
        {
            string emailAddress = "";
            string password = "";

            MimeMessage email = new();
            email.From.Add(new MailboxAddress("KnitterNotebbok", emailAddress));
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