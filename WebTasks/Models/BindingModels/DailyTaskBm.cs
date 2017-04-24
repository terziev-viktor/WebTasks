using System;

namespace WebTasks.Models.BindingModels
{
    public class DailyTaskBm
    {
        public string Title { get; set; }

        public DateTime Deadline { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }
    }
}