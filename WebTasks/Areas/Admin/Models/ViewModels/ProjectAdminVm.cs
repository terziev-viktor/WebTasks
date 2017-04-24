using System;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class ProjectAdminVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CommentsCount { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Plan { get; set; }

        public string Creator { get; set; }
    }
}