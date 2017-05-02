using System;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class ProjectAdminVm
    {
        [Required]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "The project must have a title")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "The project's description can not be more than 1000 characters long")]
        public string Description { get; set; }

        public int CommentsCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Plan { get; set; }
        
        public string Creator { get; set; }
    }
}