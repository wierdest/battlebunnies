using BattleBunnies.Api.Battles.UseCases.GetById;
using BattleBunnies.Api.Battles.UseCases.Start;
using BattleBunnies.Api.Users.UseCases.Register;
using BattleBunnies.Application.Extensions;
using BattleBunnies.Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

app.MapStartBattleEndpoint();

app.MapGetBattleByIdEndpoint();

app.MapRegisterUserEndpoint();

app.UseHttpsRedirection();

await app.RunAsync();

