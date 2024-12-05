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

            builder.HasOne(e => e.User)
                .WithMany(u => u.ChatHistories)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Messages)
                .WithOne(m => m.ChatHistory)
                .HasForeignKey(m => m.ChatHistoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Summary)
                .WithOne(s => s.ChatHistory)
                .HasForeignKey<ChatHistory>(e => e.SummaryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
