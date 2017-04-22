using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}