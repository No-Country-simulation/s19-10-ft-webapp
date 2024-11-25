using NotebookLM.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;
using NotebookLM.Mapping;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
//using AdoPet.Cloudinary;
//using AdoPet.RealTime;
//using AdoPet.SendGrid;

namespace NotebookLM.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        //builder.Services.AddApplicationServices();
        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);
        //builder.Services.AddCloudServiceExtensions(builder.Configuration);
        //builder.Services.AddRealTimeServices();
        //builder.Services.AddSendGridServiceExtensions(builder.Configuration);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwagger();
        builder.Services.AddMappingProfiles();
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed((host) => true);//agregue para probar

            });
        });

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "NotebookLM API");
        });

        app.UseHttpsRedirection();

        app.UseCors();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // RealTime 
        //app.MapHubs();
        app.MapControllers();

        return app;
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "NotebookLM",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{ }
                }
            });

            // XML comments for Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            c.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();
        });
    }

    public class SwaggerAuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                var roles = authorizeAttributes
                    .Where(attr => !string.IsNullOrEmpty(attr.Roles))
                    .Select(attr => attr.Roles)
                    .Distinct();

                var rolesText = roles.Any() ? $"Roles: {string.Join(", ", roles)}" : "Authorization required";
                operation.Description += $"<br/><b>{rolesText}</b>";
            }
        }
    }
}