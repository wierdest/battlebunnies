using BattleBunnies.EmailConfirmationMS.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServiceWorkers(builder.Configuration);

var host = builder.Build();
await host.RunAsync();
