using System;
using BattleBunnies.Contracts.Messages;
using BattleBunnies.Contracts.Queues;
using BattleBunnies.Domain.Interfaces;
using MediatR;

namespace BattleBunnies.Application.Users.UseCases.Confirm;

public class Handler(IMessagePublisher publisher) : IRequestHandler<Command, Unit>
{
    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
        await publisher.PublishAsync(
            new UserRequestedConfirmationMessage(request.Email, request.Code),
            QueueNames.UserRequestedConfirmation,
            cancellationToken
        );

        return Unit.Value;
    }
}
