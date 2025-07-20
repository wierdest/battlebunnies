using BattleBunnies.EmailConfirmationMS.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWorker(builder.Configuration);

var host = builder.Build();
await host.RunAsync();
