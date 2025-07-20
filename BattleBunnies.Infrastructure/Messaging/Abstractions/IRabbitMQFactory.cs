using System;
using RabbitMQ.Client;

namespace BattleBunnies.Infrastructure.Messaging.Abstractions;

public interface IRabbitMQFactory
{
    Task<IConnection> CreateAsync(CancellationToken cancellationToken = default);
}
