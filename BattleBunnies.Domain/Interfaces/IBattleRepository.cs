using BattleBunnies.Domain.Aggregates;

namespace BattleBunnies.Domain.Interfaces;

public interface IBattleRepository
{
    Task<Battle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(Battle battle, CancellationToken cancellationToken= default);
}
