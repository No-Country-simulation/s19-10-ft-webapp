using NotebookLM.Domain.Entities;
using NotebookLM.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotebookLM.Persistence.Identity;
using NotebookLM.Domain.Helpers;
using NotebookLM.Domain.Utilities;
using NotebookLM.Domain.Enums;
using NotebookLM.Application.Contracts.Persistence;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Options;

namespace NotebookLM.Persistence;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotebookLMDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("NotebookLMConnectionString"));
        });

        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<NotebookLMDbContext>()
            .AddDefaultTokenProviders();

        // identity
        var jwtConfig = new JwtConfiguration();
        configuration.Bind("JwtConfiguration", jwtConfig);
        services.AddSingleton(jwtConfig);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<UserManager<User>>();
        services.AddScoped<SignInManager<User>>();
        services.AddScoped<RoleManager<IdentityRole<int>>>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                ValidateIssuer = jwtConfig.ValidateIssuer,
                ValidateAudience = jwtConfig.ValidateAudience,
                ValidateLifetime = jwtConfig.ValidateLifeTime,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience
            };

            config.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/chatHub")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Role.Admin.ToStringEnum(), policy =>
                policy.RequireRole(Role.Admin.ToStringEnum()));
            options.AddPolicy(Role.User.ToStringEnum(), policy =>
                policy.RequireRole(Role.User.ToStringEnum()));    
        });        

        return services;
    }
}
