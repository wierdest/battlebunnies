using System;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Entities;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.Domain.ValueObjects;
using MediatR;

namespace BattleBunnies.Application.Battles.UseCases.Start;

public class Handler : IRequestHandler<Command, Guid>
{
    private readonly IBattleRepository _repository;

    public Handler(IBattleRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var battle = new Battle(request.OwnerId);
        var rng = new Random();

        foreach (var name in request.BunnyNames)
        {
            var pos = new Position(rng.Next(0, 10), rng.Next(0, 10));
            var bunny = new Bunny(name, pos);
            battle.AddBunny(bunny);
        }

        await _repository.SaveAsync(battle, cancellationToken);
        return battle.Id;
    }
}
