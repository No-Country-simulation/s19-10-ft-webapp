using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotebookLM.Domain.Entities;

namespace NotebookLM.Persistence.Configuration;
public class ChatHistoryConfiguration : IEntityTypeConfiguration<ChatHistory>
{
    public void Configure(EntityTypeBuilder<ChatHistory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        
        
        builder.HasMany(e => e.Messages)
            .WithOne(m => m.ChatHistory)
            .HasForeignKey(m => m.ChatHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

   
    }
}
