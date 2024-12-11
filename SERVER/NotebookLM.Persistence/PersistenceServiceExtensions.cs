using NotebookLM.Application.Contracts.Persistence;
using NotebookLM.Persistence.Data;
using NotebookLM.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotebookLM.Persistence.Services.KernelMemoryHelpers.GemmaTokenizer;
using NotebookLM.Persistence.Services.KernelMemoryHelpers;
using NotebookLM.Persistence.Services;


namespace NotebookLM.Persistence;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotebookLMDbContext>(options =>
        {
           //ptions.UseSqlServer(configuration.GetConnectionString("NotebookLMDConnectionString"));
            options.UseSqlServer(configuration["ConnectionStrings:NotebookLMDConnectionString"]);
        });

        services.AddSingleton<GemmaSentencePieceTokenizer>();
        services.AddTransient<CustomPdfDecoder>(); // Register a custom PDF decoder
        services.AddScoped<IFileService, FileService>();
        // repositories,ejempl
        //services.AddScoped<IAdoptablePetRepository, AdoptablePetRepository>();
        //services.AddScoped<IAdoptionRequestRepository, AdoptionRequestRepository>();
        return services;
    }
}
