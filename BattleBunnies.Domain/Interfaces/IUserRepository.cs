using BattleBunnies.Domain.Aggregates;

namespace BattleBunnies.Domain.Interfaces;

public interface IUserRepository
{
    Task SaveAsync(User user, CancellationToken cancellationToken = default);

}
