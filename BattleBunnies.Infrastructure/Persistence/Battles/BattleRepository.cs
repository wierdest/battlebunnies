using System;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BattleBunnies.Infrastructure.Persistence.Battles;

public class BattleRepository(DatabaseContext context) : IBattleRepository
{
    public async Task<Battle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Battles
            .Include(b => b.Bunnies)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task SaveAsync(Battle battle, CancellationToken cancellationToken = default)
    {
        await context.Battles.AddAsync(battle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
