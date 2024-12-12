using NotebookLM.Api;

var builder=WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();