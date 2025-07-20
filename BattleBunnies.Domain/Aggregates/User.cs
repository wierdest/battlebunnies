using System;
using BattleBunnies.Domain.Entities;

namespace BattleBunnies.Domain.Aggregates;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Wallet Wallet { get; private set; } = new Wallet();

    public bool IsConfirmed { get; private set; } = false;

    public DateTime? ConfirmedAt { get; private set; }

    private User() { }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        Wallet = new Wallet();
    }

    public void Confirm()
    {
        if (!IsConfirmed)
        {
            IsConfirmed = true;
            ConfirmedAt = DateTime.UtcNow;
        }
    }

}
