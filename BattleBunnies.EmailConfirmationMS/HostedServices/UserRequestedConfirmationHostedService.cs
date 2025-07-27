using System;
using BattleBunnies.Contracts.Messages;
using BattleBunnies.Contracts.Queues;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.EmailConfirmationMS.Abstractions;

namespace BattleBunnies.EmailConfirmationMS.HostedServices;

public class UserRequestedConfirmationHostedService(
    IMessageConsumer consumer,
    IConfirmationStore confirmationStore,
    IMessagePublisher publisher) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumer.ConsumeAsync<UserRequestedConfirmationMessage>(
            queueName: QueueNames.UserRequestedConfirmation,
            handleMessage: async (message) =>
            {
                var storedCode = await confirmationStore.GetConfirmationAsync(message.Email, stoppingToken);

                if (storedCode == message.Code)
                {
                    await confirmationStore.DeleteConfirmationAsync(message.Email, stoppingToken);

                    await publisher.PublishAsync(
                        new UserConfirmedMessage(message.Email),
                        QueueNames.UserConfirmed,
                        stoppingToken
                    );

                    return;
                }

                await publisher.PublishAsync(
                    new UserConfirmationFailedMessage(message.Email),
                    QueueNames.UserConfirmationFailed,
                    stoppingToken
                );
            },
            stoppingToken
        );
    }
}
