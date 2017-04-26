using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Models.EntityModels
{
    public class Project : Task
    {
        public Project()
            :base()
        {
            this.Comments = new List<Comment>();
        }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(5000)]
        public string Plan { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}