using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTasks.Models.EntityModels
{
    public class Project : Task
    {
        [Required]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(5000)]
        public string Plan { get; set; }
    }
}