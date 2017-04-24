using System;
using System.Collections.Generic;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class ProjectDetailedAdminVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Plan { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<CommentVm> Comments { get; set; }
    }
}