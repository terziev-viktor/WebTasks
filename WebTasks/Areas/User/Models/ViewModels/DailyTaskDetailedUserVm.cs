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
        
        public string Description { get; set; }
        
        public string Creator_Name { get; set; }

        public int CommentsCount { get; set; }

        public List<CommentVm> Comments;
    }
}