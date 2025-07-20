namespace BattleBunnies.Api.DTOs;

public record StartBattleDTO(Guid OwnerId, List<string> BunnyNames);