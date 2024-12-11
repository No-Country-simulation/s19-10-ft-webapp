using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Domain.Entities;

public class ChatHistory
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public int? SummaryId { get; set; }

    
}
