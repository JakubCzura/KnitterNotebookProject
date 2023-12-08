using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Settings;
using KnitterNotebook.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(SendEmailDto sendEmailDto)
    {
        //Important!
        //It would be significant issue if I store email and password in appsettings.json in open source project
        //I used it only for learning purposes and didn't send credentials to GitHub
        //Email and password are not stored in appsettings.json so sending emails will not work when you clone this repository
        EmailSettings emailSettings = _configuration.GetSection(EmailSettings.SectionKey).Get<EmailSettings>()!;

        MimeMessage email = new()
        {
            Sender = new MailboxAddress(emailSettings.SenderName, emailSettings.Email)
        };
        email.To.Add(MailboxAddress.Parse(sendEmailDto.To));
        email.Subject = sendEmailDto.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = sendEmailDto.Body };

        using SmtpClient smtp = new();
        smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(emailSettings.Email, emailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}