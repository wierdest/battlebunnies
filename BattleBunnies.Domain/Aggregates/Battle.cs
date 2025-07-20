using BattleBunnies.Domain.Entities;

namespace BattleBunnies.Domain.Aggregates;

public class Battle
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public List<Guid> OwnerIds { get; private set; } = [];
    public List<Bunny> Bunnies { get; private set; } = [];
    public int CurrentTurn { get; private set; } = 1;

    public Battle(Guid ownerId)
    {
        AddOwner(ownerId);
    }

    private Battle() {}

    public void AddOwner(Guid userId)
    {
        if (!OwnerIds.Contains(userId))
            OwnerIds.Add(userId);
    }

    public void AddBunny(Bunny bunny)
    {
        Bunnies.Add(bunny);
    }

    public void NextTurn()
    {
        CurrentTurn++;
    }

    public Bunny? GetBunny(Guid unitId) => Bunnies.FirstOrDefault(u => u.Id == unitId);
    

}
