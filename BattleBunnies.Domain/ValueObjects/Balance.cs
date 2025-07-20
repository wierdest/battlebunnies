namespace BattleBunnies.Domain.ValueObjects;

public record class Balance(decimal Amount)
{
    public static Balance Zero => new(0);

    public Balance Add(decimal value) => new(Amount + value);

    public Balance Substract(decimal value)
    {
        if (value > Amount) throw new InvalidOperationException("Insufficient Funds");
        return new(Amount - value);
    }

    public bool IsPositive => Amount > 0;
}
