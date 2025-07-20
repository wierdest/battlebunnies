using BattleBunnies.Domain.Entities;

namespace BattleBunnies.Api.DTOs;

public record BunnyDTO(Guid Id, string Name, int Health, int X, int Y)
{
    public static BunnyDTO FromDomain(Bunny bunny) => new(
        bunny.Id,
        bunny.Name,
        bunny.Health,
        bunny.Position.X,
        bunny.Position.Y
    );
}