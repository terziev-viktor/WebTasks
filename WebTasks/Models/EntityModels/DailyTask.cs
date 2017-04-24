using System;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Models.EntityModels
{
    public class DailyTask : Task
    {
        public string Note { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
    }
}