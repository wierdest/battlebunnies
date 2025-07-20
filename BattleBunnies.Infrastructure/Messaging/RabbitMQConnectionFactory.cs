using System;
using BattleBunnies.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BattleBunnies.Infrastructure.Messaging;

public class RabbitMQConnectionFactory() : IRabbitMQFactory
{
    private readonly ConnectionFactory factory = new ConnectionFactory
    {
        HostName = "localhost"
    };
    public async Task<IConnection> CreateAsync(CancellationToken cancellationToken = default) => await factory.CreateConnectionAsync(cancellationToken);
}
