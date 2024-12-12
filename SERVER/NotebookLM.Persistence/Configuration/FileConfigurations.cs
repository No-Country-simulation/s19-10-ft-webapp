using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotebookLM.Domain.Entities;

namespace NotebookLM.Persistence.Configuration;
internal class FileConfigurations : IEntityTypeConfiguration<Domain.Entities.File>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.File> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        

        builder.HasOne(e => e.User)
            .WithMany(u => u.Files)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
