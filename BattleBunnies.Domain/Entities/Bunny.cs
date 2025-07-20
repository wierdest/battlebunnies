using BattleBunnies.Domain.ValueObjects;

namespace BattleBunnies.Domain.Entities;

public class Bunny
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public int Health { get; private set; }
    public Position Position { get; private set; } = new Position(0, 0);
    private Bunny() { }
    public Bunny(string name, Position position)
    {
        Name = name;
        Position = position;
        Health = 100;
    }
    public void MoveTo(Position newPosition) => Position = newPosition;
    public void TakeDamage(int amount)
    {
        Health = Math.Max(Health - amount, 0);
    }
    public bool IsAlive => Health > 0;

}
