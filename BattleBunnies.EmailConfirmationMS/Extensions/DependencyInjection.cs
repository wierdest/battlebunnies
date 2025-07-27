using System;
using BattleBunnies.EmailConfirmationMS.Abstractions;
using BattleBunnies.EmailConfirmationMS.HostedServices;
using BattleBunnies.EmailConfirmationMS.Services;
using BattleBunnies.EmailConfirmationMS.Settings;
using BattleBunnies.EmailConfirmationMS.Stores;
using BattleBunnies.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BattleBunnies.EmailConfirmationMS.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceWorkers(this IServiceCollection services, IConfiguration config)
    {
        services.AddMessagingInfrastructure();

        services.Configure<ConfirmationSettings>(config.GetSection("Confirmation"));

        services.Configure<SMTPSettings>(config.GetSection("SMTP"));

        services.AddSingleton<IEmailSender, SMTPEmailSender>();

        services.AddSingleton<ICodeGenerator, CodeGenerator>();

        services.AddSingleton<IConfirmationLinkFactory, ConfirmationLinkFactory>();

        services.Configure<RedisSettings>(config.GetSection("Redis"));

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            return ConnectionMultiplexer.Connect(settings.ConnectionString);
        });


        services.AddSingleton<IConfirmationStore, ConfirmationStore>();
        
        services.AddHostedService<UserRegisteredHostedService>();

        services.AddHostedService<UserRequestedConfirmationHostedService>();

        return services;
    }
}
