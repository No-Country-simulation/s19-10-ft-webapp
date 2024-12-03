using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotebookLM.Domain.Entities;

namespace NotebookLM.Persistence.Configuration;
internal class FileConfigurations : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.HasOne(e => e.User)
            .WithMany(u => u.Files)
            .HasForeignKey<File>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
