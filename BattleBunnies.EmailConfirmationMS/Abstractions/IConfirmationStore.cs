using System;

namespace BattleBunnies.EmailConfirmationMS.Abstractions;

public interface IConfirmationStore
{
    Task StoreConfirmationAsync(string email, string code, CancellationToken cancellationToken);
    Task<string?> GetConfirmationAsync(string email, CancellationToken cancellationToken);
    Task DeleteConfirmationAsync(string email, CancellationToken cancellationToken);
}
