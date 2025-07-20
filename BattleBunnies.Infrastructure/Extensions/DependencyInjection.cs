using System;
using System.Threading.Tasks;
using BattleBunnies.Domain.Interfaces;
using BattleBunnies.Infrastructure.Messaging;
using BattleBunnies.Infrastructure.Messaging.Abstractions;
using BattleBunnies.Infrastructure.Persistence;
using BattleBunnies.Infrastructure.Persistence.Battles;
using BattleBunnies.Infrastructure.Persistence.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BattleBunnies.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));

        services.AddScoped<IBattleRepository, BattleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddMessagingInfrastructure();

        return services;
    }

    public static IServiceCollection AddMessagingInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMQFactory, RabbitMQConnectionFactory>();
        services.AddSingleton<IMessagePublisher, RabbitMQMessagePublisher>();
        services.AddSingleton<IMessageConsumer, RabbitMQMessageConsumer>();

        return services;
    }
}
