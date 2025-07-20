using MediatR;
using BattleBunnies.Application.Users.UseCases.Confirm;

namespace BattleBunnies.Api.Users.UseCases.Confirm;

public static class Endpoint
{
    public static void MapConfirmUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/confirm", async (Command command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Accepted();
        });
    }

}
