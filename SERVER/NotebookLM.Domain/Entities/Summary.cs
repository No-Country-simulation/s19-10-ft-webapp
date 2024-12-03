using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotebookLM.Domain.Entities;

   public class Summary
   {
       public int Id { get; set; }
       
       public int FileId { get; set; }
       
       public string SummaryPath { get; set; } = null!;
       
       public decimal ApiProccessingTime { get; set; }
       
       public string? UserFeedBack { get; set; }
       
       public DateTime CreatedAt { get; set; }
       
       public DateTime UpdatedAt { get; set; }
       
       public virtual File File { get; set; } = null!;
   }