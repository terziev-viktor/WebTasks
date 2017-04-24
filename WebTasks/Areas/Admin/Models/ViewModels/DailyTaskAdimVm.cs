using System;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class DailyTaskAdimVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public int CommentsCount { get; set; }

        public string Creator { get; set; }

    }
}