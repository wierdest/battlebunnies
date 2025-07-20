using System;

namespace BattleBunnies.Domain.Interfaces;

public interface IMessageConsumer
{
    Task ConsumeAsync<T>(
        string queueName,
        Func<T, Task> handleMessage,
        CancellationToken cancellationToken = default);
}
