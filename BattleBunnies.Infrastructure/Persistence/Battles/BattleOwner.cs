using BattleBunnies.Domain.Aggregates;

namespace BattleBunnies.Infrastructure.Persistence.Battles;
// This is a join entity
public class BattleOwner
{
    public Guid BattleId { get; set; }
    public Guid UserId { get; set; }

    public Battle? Battle { get; set; }
    public User? User { get; set; } 
}
