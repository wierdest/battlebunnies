using System;

namespace BattleBunnies.EmailConfirmationMS.Abstractions;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}
