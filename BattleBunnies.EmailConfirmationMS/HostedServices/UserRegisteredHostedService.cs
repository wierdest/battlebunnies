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
    IConfirmationLinkFactory confirmationLinkFactory,
    IConfirmationStore confirmationStore) :  BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumer.ConsumeAsync<UserRegisteredMessage>(
            queueName: QueueNames.UserRegistered,
            handleMessage: async (message) =>
            {
                var code = codeGenerator.Generate();

                var link = confirmationLinkFactory.Create(message.Email, code);

                await emailSender.SendAsync(
                    message.Email,
                    "Confirm your battlebunnies access: ",
                    $"Click the link to confirm:\n${link}"
                );

                await confirmationStore.StoreConfirmationAsync(message.Email, code, stoppingToken);

            },
            stoppingToken
        );
    }
}
