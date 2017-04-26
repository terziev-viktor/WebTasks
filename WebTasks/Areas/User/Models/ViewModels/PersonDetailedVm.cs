using System.Collections.Generic;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.User.Models.ViewModels
{
    public class PersonDetailedVm
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        
        public int TasksCount { get; set; }

        public int DailyTasksCount { get; set; }

        public int ProjectsCount { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<DailyTaskVm> DailyTasks { get; set; }

        public IEnumerable<ProjectVm> Projects { get; set; }


    }
}