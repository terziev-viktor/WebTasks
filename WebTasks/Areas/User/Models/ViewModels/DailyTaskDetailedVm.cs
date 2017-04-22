namespace WebTasks.Areas.User.Models.ViewModels
{
    public class DailyTaskDetailedVm
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Creator_Name { get; set; }

        public int CommentsCount { get; set; }
    }
}