using System;
using System.ComponentModel.DataAnnotations;

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