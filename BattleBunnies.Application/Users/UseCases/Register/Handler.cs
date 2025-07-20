using System.Security.Cryptography;
using System.Text;
using BattleBunnies.Contracts.Messages;
using BattleBunnies.Contracts.Queues;
using BattleBunnies.Domain.Aggregates;
using BattleBunnies.Domain.Interfaces;
using MediatR;

namespace BattleBunnies.Application.Users.UseCases.Register;

public class Handler(IUserRepository repository, IMessagePublisher publisher) : IRequestHandler<Command, Guid>
{
    public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(request.Password)));
        var user = new User(request.Email, hash);
        
        await repository.SaveAsync(user, cancellationToken);

        await publisher.PublishAsync(
            new UserRegisteredMessage(user.Email),
            QueueNames.UserRegistered,
            cancellationToken
        );

        return user.Id;
    }
}
