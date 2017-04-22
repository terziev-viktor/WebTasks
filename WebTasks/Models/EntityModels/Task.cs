using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebTasks.Models.EntityModels
{
    public class Task
    {
        public Task()
        {
            this.Comments = new List<Comment>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public ApplicationUser Creator { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}