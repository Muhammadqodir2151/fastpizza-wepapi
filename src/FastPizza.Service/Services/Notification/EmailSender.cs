﻿using FastPizza.Service.Dtos.Notifications;
using FastPizza.Service.Interfaces.Notifications;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;

namespace FastPizza.Service.Services.Notification;

public class EmailSender : IEmailsender
{
    private readonly string SENDER = string.Empty;
    private readonly string PLATFORM = string.Empty;
    private readonly string PORT = string.Empty;
    private readonly string PASSWORD = string.Empty;
    public EmailSender(IConfiguration configuration)
    {
        SENDER = configuration["Email:SENDER"];
        PLATFORM = configuration["Email:Platform"];
        PORT = configuration["Email:Port"];
        PASSWORD = configuration["Email:Password"];

    }
    public async Task<bool> SenderAsync(EmailMessage emailMessage)
    {
        try
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(SENDER));
            mail.To.Add(MailboxAddress.Parse(emailMessage.Recipent));
            mail.Subject = emailMessage.Title;
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content.ToString()
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(PLATFORM, int.Parse(PORT), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(SENDER, PASSWORD);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
