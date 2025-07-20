
using MediatR;
using BattleBunnies.Application.Battles.UseCases.GetById;
using BattleBunnies.Api.DTOs;

namespace BattleBunnies.Api.Battles.UseCases.GetById;

public static class Endpoint
{
    public static void MapGetBattleByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/battles/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var battle = await mediator.Send(new Query(id));

            if (battle is null) return Results.NotFound();

            return Results.Ok(BattleDTO.FromDomain(battle));
        });
    }
}
