using MediatR;
using BattleBunnies.Application.Users.UseCases.Register;

namespace BattleBunnies.Api.Users.UseCases.Register;

public static class Endpoint
{
    public static void MapRegisterUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/register", async (Command command, IMediator mediator) =>
        {
            var userId = await mediator.Send(command);
            return Results.Ok(userId);
        });
    }

}
