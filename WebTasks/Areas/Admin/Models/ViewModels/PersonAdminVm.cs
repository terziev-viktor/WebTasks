namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class PersonAdminVm
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int TasksCount { get; set; }

        public int DailyTasksCount { get; set; }

        public int ProjectsCount { get; set; }
        
    }
}