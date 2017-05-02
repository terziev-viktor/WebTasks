using System.Collections.Generic;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class PersonDetailedAdminVm
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public string Email { get; set; }

        public int TasksCount { get; set; }

        public int DailyTasksCount { get; set; }

        public int ProjectsCount { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<DailyTaskVm> DailyTasks { get; set; }

        public IEnumerable<ProjectVm> Projects { get; set; }

        public IEnumerable<CommentVm> Comments { get; set; }

        public int AccessFailedCount { get; set; }

        public string PhoneNumber { get; set; }
    }
}