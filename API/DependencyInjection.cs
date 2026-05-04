using API.Services;
using Application.Common.Interfaces;
using Application.Common.Settings;
using System.Text.Json.Serialization;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllerWithJsonConfig()
                .AddCorsConfig()
                .AddOutputCachingConfig();

        services.Configure<PayPalSettings>(configuration.GetSection(nameof(PayPalSettings)));
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSignalR();
        
        return services;
    }
    private static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MyAppPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();    
            });
        });
        return services;
    }
    private static IServiceCollection AddOutputCachingConfig(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.SizeLimit = 100 * 1024 * 1024;
            options.AddBasePolicy(policy =>
            {
                policy.Expire(TimeSpan.FromSeconds(60));
            });
        });

        return services;
    }
    public static IServiceCollection AddControllerWithJsonConfig(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options => options
            .JsonSerializerOptions
            .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        return services;
    }
}
