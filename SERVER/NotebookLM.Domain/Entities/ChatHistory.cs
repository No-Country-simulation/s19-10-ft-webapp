using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Domain.Entities;

public class ChatHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    public int? SummaryId { get; set; }
    public virtual Summary? Summary { get; set; }
}
