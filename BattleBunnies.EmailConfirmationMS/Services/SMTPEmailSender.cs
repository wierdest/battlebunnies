using System;
using System.Net;
using System.Net.Mail;
using BattleBunnies.EmailConfirmationMS.Abstractions;
using BattleBunnies.EmailConfirmationMS.Settings;
using Microsoft.Extensions.Options;

namespace BattleBunnies.EmailConfirmationMS.Services;

public class SMTPEmailSender(IOptions<SMTPSettings> options) : IEmailSender
{
    private readonly SMTPSettings _settings = options.Value;
    public async Task SendAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password)
        };

        using var message = new MailMessage(_settings.From, to, subject, body);

        await client.SendMailAsync(message);
    }
}
