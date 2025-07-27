using System;

namespace BattleBunnies.Contracts.Queues;

public static class QueueNames
{
    public const string UserRegistered = "user-registered";
    public const string UserRequestedConfirmation = "user-requested-confirmation";
    public const string UserConfirmed = "user-confirmed";
    public const string UserConfirmationFailed = "user-confirmation-failed";

}
