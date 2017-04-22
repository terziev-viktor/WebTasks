using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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