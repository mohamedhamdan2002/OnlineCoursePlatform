using Application.Common.Interfaces;
using Domain.Identity;
using Infrastructure.Data;
using Infrastructure.RealTime;
using Infrastructure.Services;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        //services.AddIdentity<User, IdentityRole<Guid>>()
        //    .AddEntityFrameworkStores<AppDbContext>()
        //    .AddDefaultTokenProviders();

        //services.AddAuthentication(opt =>
        //{
        //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //}).AddJwtBearer(options =>
        //{
        //    var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = true,
        //        ValidateIssuer = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = jwtSettings?.ValidIssuer,
        //        ValidAudience = jwtSettings?.ValidIssuer,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.ValidIssuer!))
        //    };
        //});
        services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(10), // L2, L3
            LocalCacheExpiration = TimeSpan.FromSeconds(30), // L1
        });
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IPayPalService, PayPalService>();
        services.AddScoped<IEnrollmentNotifier, SignalREnrollmentNotifier>();
        services.AddHttpClient();
        return services;
    }
}
