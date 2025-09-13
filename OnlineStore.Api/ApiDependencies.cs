using OnlineStore.Api.utils;
using OnlineStore.Application.Providers;
using Serilog;

namespace OnlineStore.Api;

public static class ApiDependencies
{
    public static IServiceCollection AddApiApiDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ILoggedInUser, LoggedInUser>();

        return services;
    }
}
