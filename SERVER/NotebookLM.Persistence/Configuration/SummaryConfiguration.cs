using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotebookLM.Domain.Entities;

namespace NotebookLM.Persistence.Configuration;
    public class SummaryConfiguration : IEntityTypeConfiguration<Summary>
    {
        public void Configure(EntityTypeBuilder<Summary> builder)
        {
            
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.HasOne(e => e.File)
                .WithMany(f => f.Summaries)
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
