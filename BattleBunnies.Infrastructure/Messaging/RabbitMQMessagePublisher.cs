
using System.Text.Json;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.Infrastructure.Messaging.Abstractions;
using RabbitMQ.Client;

namespace BattleBunnies.Infrastructure.Messaging;

public class RabbitMQMessagePublisher(IRabbitMQFactory factory) : IMessagePublisher
{
    public async Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default)
    {
        var connection = await factory.CreateAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        using (channel)
        {
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken
            );
        }

    }
}
