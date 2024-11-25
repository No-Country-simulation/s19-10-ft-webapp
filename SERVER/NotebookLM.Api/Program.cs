using NotebookLM.Api;

var builder=WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();