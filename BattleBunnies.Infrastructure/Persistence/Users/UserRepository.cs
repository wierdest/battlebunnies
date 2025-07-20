using System;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Interfaces;

namespace BattleBunnies.Infrastructure.Persistence.Users;

public class UserRepository(DatabaseContext context) : IUserRepository
{
    public async Task SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
