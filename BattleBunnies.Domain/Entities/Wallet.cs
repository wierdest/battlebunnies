using System;
using BattleBunnies.Domain.ValueObjects;

namespace BattleBunnies.Domain.Entities;

public class Wallet
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Balance Balance { get; private set; } = Balance.Zero;
    public void Credit(decimal amount) => Balance = Balance.Add(amount);
    public void Debit(decimal amount) => Balance = Balance.Substract(amount);

}
