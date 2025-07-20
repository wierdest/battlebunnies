using MediatR;

namespace BattleBunnies.Application.Battles.UseCases.Start;

public record class Command(Guid OwnerId, List<string> BunnyNames) : IRequest<Guid>;
