using OnlineStore.Api.utils;
using OnlineStore.Application.Mapping;
using OnlineStore.Application.Providers;
using Serilog;

namespace OnlineStore.Api;

public static class ApiDependencies
{
    public static IServiceCollection AddApiApiDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ILoggedInUser, LoggedInUser>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        MapsterConfig.RegisterMappings();

        return services;
    }
}
