using System;

namespace WebTasks.Models.ViewModels
{
    public class CommentVm
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }
}