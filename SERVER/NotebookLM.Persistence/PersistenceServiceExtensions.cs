using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Persistence.Data;
using NotebookLM.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace NotebookLM.Persistence;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotebookLMDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("NotebookLMDConnectionString"));
        });

        // repositories,ejempl
        //services.AddScoped<IAdoptablePetRepository, AdoptablePetRepository>();
        //services.AddScoped<IAdoptionRequestRepository, AdoptionRequestRepository>();
        return services;
    }
}
