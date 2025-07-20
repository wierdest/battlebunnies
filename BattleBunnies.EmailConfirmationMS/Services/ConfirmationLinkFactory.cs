using System;
using BattleBunnies.EmailConfirmationMS.Abstractions;
using BattleBunnies.EmailConfirmationMS.Settings;
using Microsoft.Extensions.Options;

namespace BattleBunnies.EmailConfirmationMS.Services;

public class ConfirmationLinkFactory(IOptions<ConfirmationSettings> options) : IConfirmationLinkFactory
{
    private readonly string _baseUrl = options.Value.ConfirmationBaseURL;
    public string Create(string email, string code)
    {
        var encodedEmail = Uri.EscapeDataString(email);
        var encodedCode = Uri.EscapeDataString(code);

        return $"{_baseUrl}?email=${encodedEmail}&code={encodedCode}";
    }
}
