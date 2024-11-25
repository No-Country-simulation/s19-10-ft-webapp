using NotebookLM.Domain.Enums;
using NotebookLM.Domain.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NotebookLM.Persistence.Seeding;

public static class Seed
{
    public static void IntialSeed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = Role.Admin.ToStringEnum(), NormalizedName = Role.Admin.ToStringEnum().ToUpper() },
            new IdentityRole<int> { Id = 2, Name = Role.User.ToStringEnum(), NormalizedName = Role.User.ToStringEnum().ToUpper() }
        );
    }
}
