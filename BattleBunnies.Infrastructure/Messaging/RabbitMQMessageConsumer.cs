using System.Text;
using System.Text.Json;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.Infrastructure.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BattleBunnies.Infrastructure.Messaging;

public class RabbitMQMessageConsumer(IRabbitMQFactory factory, ILogger<RabbitMQMessageConsumer> logger) : IMessageConsumer
{
    public async Task ConsumeAsync<T>(
        string queueName,
        Func<T, Task> handleMessage,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting to consume queue: {Queue}", queueName);
        var connection = await factory.CreateAsync(cancellationToken);
        logger.LogInformation("RabbitMQ connection established.");
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        logger.LogInformation("RabbitMQ channel created.");

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken
        );
        logger.LogInformation("Queue '{Queue}' declared.", queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(body);
                logger.LogInformation("Message received from queue '{Queue}': {Body}", queueName, Encoding.UTF8.GetString(body));
                if (message is not null)
                {
                    await handleMessage(message);
                    logger.LogInformation("Message handled successfully.");
                }
                else
                {
                    logger.LogWarning("Deserialized message is null.");
                }

                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error processing message from queue '{Q}'", queueName);
            }
        };
        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
        logger.LogInformation("Consumer started for queue '{Q}'", queueName);
    }
}
