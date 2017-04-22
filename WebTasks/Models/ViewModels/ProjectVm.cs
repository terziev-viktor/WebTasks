using System;

namespace WebTasks.Models.ViewModels
{
    public class ProjectVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CommentsCount { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}