using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Settings;
using KnitterNotebook.Services.Interfaces;
using MailKit.Net.Smtp;
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
        //Due to security concern I didn't send email account credentials to GitHub so sending emails will not work when you clone this repository
        //I give the credentials only to people who I have created the application for
        EmailSettings emailSettings = _configuration.GetSection(EmailSettings.SectionKey).Get<EmailSettings>()!;
        SmtpSettings smtpSettings = _configuration.GetSection(SmtpSettings.SectionKey).Get<SmtpSettings>()!;

        MimeMessage email = new()
        {
            Sender = new MailboxAddress(emailSettings.SenderName, emailSettings.Email)
        };
        email.To.Add(MailboxAddress.Parse(sendEmailDto.To));
        email.Subject = sendEmailDto.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = sendEmailDto.Body };

        using SmtpClient smtp = new();
        await smtp.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.SecureSocketOptions);
        await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}