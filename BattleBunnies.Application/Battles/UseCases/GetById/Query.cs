using System;
using BattleBunnies.Domain.Aggregates;
using MediatR;

namespace BattleBunnies.Application.Battles.UseCases.GetById;

public record Query(Guid Id) : IRequest<Battle?>;
