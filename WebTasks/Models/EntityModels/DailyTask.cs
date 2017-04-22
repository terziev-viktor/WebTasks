using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTasks.Models.EntityModels
{
    public class DailyTask : Task
    {
        public string Note { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
    }
}