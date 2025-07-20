using MediatR;

namespace BattleBunnies.Application.Users.UseCases.Confirm;

public record class Command(string Email, string Code) : IRequest<Unit>;