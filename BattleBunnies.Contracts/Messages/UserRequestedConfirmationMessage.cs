namespace BattleBunnies.Contracts.Messages;

public record class UserRequestedConfirmationMessage(string Email, string Code);