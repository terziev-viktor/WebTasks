using System;
using WebTasks.Helpers;

namespace WebTasks.Models.ViewModels
{
    public class CommentVm
    {
        private string _content;

        public int Id { get; set; }

        public string Author { get; set; }

        public string Content { get { return HtmlSerializer.ToDecodedString(_content); } set { _content = value; } }

        public DateTime PublishDate { get; set; }
    }
}