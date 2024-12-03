using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Persistence.Configuration;
internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.HasOne(e => e.ChatHistory)
            .WithMany(ch => ch.Messages)
            .HasForeignKey(e => e.ChatHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
