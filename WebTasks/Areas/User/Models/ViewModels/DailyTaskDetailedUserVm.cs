using System;
using System.Collections.Generic;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.User.Models.ViewModels
{
    public class DailyTaskDetailedUserVm
    {
        public DailyTaskDetailedUserVm()
        {
            this.Comments = new List<CommentVm>();
        }

        public int Id { get; set; }
        
        public string Title { get; set; }

        public string Note { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }
        
        public int CommentsCount { get; set; }

        public List<CommentVm> Comments;
    }
}