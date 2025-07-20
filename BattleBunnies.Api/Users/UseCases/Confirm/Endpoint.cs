using MediatR;
using BattleBunnies.Application.Users.UseCases.Confirm;
using Microsoft.AspNetCore.Mvc;

namespace BattleBunnies.Api.Users.UseCases.Confirm;

public static class Endpoint
{
    public static void MapConfirmUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/confirm", async ([FromQuery] string email, [FromQuery] string code, IMediator mediator) =>
        {
            var command = new Command(email, code);

            await mediator.Send(command);

            return Results.Accepted();
        })
        .ExcludeFromDescription();
    }

}
