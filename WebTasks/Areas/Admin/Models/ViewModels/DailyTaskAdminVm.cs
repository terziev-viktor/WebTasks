using System;
using System.ComponentModel.DataAnnotations;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class DailyTaskAdminVm
    {
        [Required]
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "The task must have a title")]
        public string Title { get; set; }

        public string Note { get; set; }

        public int CommentsCount { get; set; }
        
        public string Creator { get; set; }

    }
}