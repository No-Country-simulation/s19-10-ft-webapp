using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int ChatHistoryId { get; set; }
        public virtual ChatHistory ChatHistory { get; set; } = null!;
    }
}
