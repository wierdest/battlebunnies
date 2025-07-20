using MediatR;

namespace BattleBunnies.Application.Users.UseCases.Register;

public record Command(string Email, string Password) : IRequest<Guid>;
