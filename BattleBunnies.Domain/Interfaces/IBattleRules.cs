using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.ValueObjects;

namespace BattleBunnies.Domain.Interfaces;

public interface IBattleRules
{
    bool CanMove(Battle battle, Guid bunnyId, Position targetPosition);
    bool CanAttack(Battle battle, Guid attackerId, Guid targetId);

}
