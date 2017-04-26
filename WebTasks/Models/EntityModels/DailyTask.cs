using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Models.EntityModels
{
    public class DailyTask : Task
    {
        public DailyTask()
            :base()
        {
            this.Comments = new List<Comment>();
        }

        public string Note { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}