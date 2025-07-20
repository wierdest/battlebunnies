using System;
using BattleBunnies.Contracts.Messages;
using BattleBunnies.Contracts.Queues;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.EmailConfirmationMS.Abstractions;


namespace BattleBunnies.EmailConfirmationMS.HostedServices;

public class UserRegisteredHostedService(
    IMessageConsumer consumer,
    IEmailSender emailSender,
    ICodeGenerator codeGenerator,
    IConfirmationStore confirmationStore) :  BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumer.ConsumeAsync<UserRegisteredMessage>(
            queueName: QueueNames.UserRegistered,
            handleMessage: async (message) =>
            {
                var code = codeGenerator.Generate();

                await emailSender.SendAsync(message.Email, "This is your confirmation email", code);

                await confirmationStore.StoreConfirmationAsync(message.Email, code, stoppingToken);

            },
            stoppingToken
        );
    }
}
