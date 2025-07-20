using BattleBunnies.Domain.Aggregates;

namespace BattleBunnies.Api.DTOs;

public record class BattleDTO(Guid Id, int Turn, List<BunnyDTO> Bunnies)
{
    public static BattleDTO FromDomain(Battle battle) => new(
        battle.Id,
        battle.CurrentTurn,
        [.. battle.Bunnies.Select(BunnyDTO.FromDomain)]
    );
}