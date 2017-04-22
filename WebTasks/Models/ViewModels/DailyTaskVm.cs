namespace WebTasks.Models.ViewModels
{
    public class DailyTaskVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public int CommentsCount { get; set; }
    }
}