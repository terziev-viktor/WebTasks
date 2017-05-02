using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Models.EntityModels
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public virtual ApplicationUser Creator { get; set; }
    }
}