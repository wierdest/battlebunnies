using MediatR;
using BattleBunnies.Application.Battles.UseCases.Start;
using BattleBunnies.Api.DTOs;

namespace BattleBunnies.Api.Battles.UseCases.Start;

public static class Endpoint
{
    public static void MapStartBattleEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/battles/start", async (StartBattleDTO dto, IMediator mediator) =>
        {
            var command = new Command(dto.OwnerId, dto.BunnyNames);
            var result = await mediator.Send(command);
            return Results.Created($"/api/battles/{result}", result);
        });
    }
}
