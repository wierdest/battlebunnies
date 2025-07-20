using System;

namespace BattleBunnies.EmailConfirmationMS.Abstractions;

public interface IConfirmationStore
{
    Task StoreConfirmationAsync(string email, string code, CancellationToken cancellationToken);
}
