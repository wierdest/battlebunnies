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
    public async Task StoreConfirmationAsync(string email, string code, CancellationToken cancellationToken)
    {
        await _db.StringSetAsync($"confirm:{email}", code, _ttl).WaitAsync(cancellationToken);
    }
}
