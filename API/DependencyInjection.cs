using API.Services;
using Application.Common.Interfaces;
using Application.Common.Settings;
using System.Runtime.CompilerServices;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddCorsConfig();
        services.Configure<PayPalSettings>(configuration.GetSection(nameof(PayPalSettings)));
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
    
    private static void AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MyAppPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }
}
