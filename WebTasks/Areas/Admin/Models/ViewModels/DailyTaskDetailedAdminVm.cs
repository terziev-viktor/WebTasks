using System;
using System.Collections.Generic;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class DailyTaskDetailedAdminVm
    {
        public DailyTaskDetailedAdminVm()
        {
            this.Comments = new List<CommentVm>();
        }

        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Creator { get; set; }

        public ICollection<CommentVm> Comments { get; set; }

        public int CommentsCount { get; set; }

        public string Note { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}