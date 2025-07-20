using System;

namespace BattleBunnies.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default);
}
