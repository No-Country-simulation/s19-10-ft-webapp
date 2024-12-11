using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Domain.Entities;
   public class File
   {
       public int Id { get; set; }
       
       public int UserId { get; set; }
       
       public string FilePath { get; set; } = null!;
       
       public string FileName { get; set; } = null!;
       
       public DateTime CreatedAt { get; set; }
       
       public DateTime UpdatedAt { get; set; }

        public int chatHistoryId { get; set; }  

        public virtual User User { get; set; } = null!;

       public virtual ICollection<Summary> Summaries { get; set; } = new List<Summary>();

   }