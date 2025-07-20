using System;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Interfaces;
using MediatR;

namespace BattleBunnies.Application.Battles.UseCases.GetById;

public class Handler(IBattleRepository repository) : IRequestHandler<Query, Battle?>
{
    public async Task<Battle?> Handle(Query request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
