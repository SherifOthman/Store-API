using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Domain.Interfaces;
using OnlineStore.Infrastructure.Repositories;

namespace OnlineStore.Infrastructure;
public static class InfrastrcureDependencies
{
    public static IServiceCollection AddInfrastrcureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<IUnitOfWork>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connString = configuration.GetConnectionString("Default");

            ArgumentNullException.ThrowIfNullOrEmpty(connString);

            return new UnitOfWork(connString);
        });

    }
}
