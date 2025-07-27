using System;
using BattleBunnies.EmailConfirmationMS.Abstractions;
using BattleBunnies.EmailConfirmationMS.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BattleBunnies.EmailConfirmationMS.Stores;

public class ConfirmationStore(
    IConnectionMultiplexer redis,
    IOptions<RedisSettings> options) : IConfirmationStore
{
    private readonly IDatabase _db = redis.GetDatabase();
    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(options.Value.TtlMinutes);

    private static string GetKey(string email) => $"confirm:{email}";

    public async Task DeleteConfirmationAsync(string email, CancellationToken cancellationToken)
    {
        await _db.KeyDeleteAsync(GetKey(email)).WaitAsync(cancellationToken);
    }

    public async Task<string?> GetConfirmationAsync(string email, CancellationToken cancellationToken)
    {
        var result = await _db.StringGetAsync(GetKey(email)).WaitAsync(cancellationToken);
        return result.HasValue ? result.ToString() : null;
    }

    public async Task StoreConfirmationAsync(string email, string code, CancellationToken cancellationToken)
    {
        await _db.StringSetAsync(GetKey(email), code, _ttl).WaitAsync(cancellationToken);
    }
}
