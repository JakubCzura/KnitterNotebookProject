﻿using KnitterNotebook.Models.Dtos;
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