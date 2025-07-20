using System;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Entities;
using BattleBunnies.Infrastructure.Persistence.Battles;
using Microsoft.EntityFrameworkCore;

namespace BattleBunnies.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Battle> Battles => Set<Battle>();
    public DbSet<Bunny> Bunnies => Set<Bunny>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Wallet, wallet =>
            {
                wallet.OwnsOne(w => w.Balance);
            });
        
        modelBuilder.Entity<Battle>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Battle>()
            .Ignore(b => b.OwnerIds);

        modelBuilder.Entity<BattleOwner>()
            .HasKey(x => new { x.BattleId, x.UserId });

        modelBuilder.Entity<BattleOwner>()
            .HasOne(x => x.Battle)
            .WithMany()
            .HasForeignKey(x => x.BattleId);

        modelBuilder.Entity<BattleOwner>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Battle>()
            .HasMany(b => b.Bunnies)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Bunny>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Bunny>()
            .OwnsOne(b => b.Position);
        
    }
}
