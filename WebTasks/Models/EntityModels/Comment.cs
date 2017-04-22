using System;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Models.EntityModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        public string Content { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }
    }
}