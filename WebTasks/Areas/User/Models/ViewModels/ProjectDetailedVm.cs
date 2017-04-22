using System;

namespace WebTasks.Areas.User.Models.ViewModels
{
    public class ProjectDetailedVm
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Creator { get; set; }
        
        public DateTime ReleaseDate { get; set; }
        
        public string Plan { get; set; }

        public int CommentsCount { get; set; }
    }
}