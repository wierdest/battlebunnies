using System;
using System.Security.Cryptography;
using System.Text;
using BattleBunnies.EmailConfirmationMS.Abstractions;
using BattleBunnies.EmailConfirmationMS.Settings;
using Microsoft.Extensions.Options;

namespace BattleBunnies.EmailConfirmationMS.Services;

public class CodeGenerator(IOptions<ConfirmationSettings> options) : ICodeGenerator
{
    private readonly byte[] _secret = Encoding.UTF8.GetBytes(options.Value.CodeSecretKey);
    public string Generate(int length = 32)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var nonce = Guid.NewGuid().ToString("N");
        var payload = $"{now}:{nonce}";
        using var hmac = new HMACSHA256(_secret);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var base64 = Convert.ToBase64String(hashBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        
        return base64[..Math.Min(length, base64.Length)];
    }
}
