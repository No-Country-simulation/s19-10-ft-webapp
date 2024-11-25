using Microsoft.EntityFrameworkCore;
using System.Reflection;
using NotebookLM.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NotebookLM.Persistence.Data;

public class NotebookLMDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public NotebookLMDbContext(DbContextOptions<NotebookLMDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        Seeding.Seed.IntialSeed(builder);
    }
}
