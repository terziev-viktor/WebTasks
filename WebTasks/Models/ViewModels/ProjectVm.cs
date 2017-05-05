using System;
using WebTasks.Helpers;

namespace WebTasks.Models.ViewModels
{
    public class ProjectVm
    {
        private string _title;
        private string _desc;
        private string _plan;

        public int Id { get; set; }

        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }

        public string Description { get { return HtmlSerializer.ToDecodedString(_desc); } set { _desc = value; } }

        public int CommentsCount { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Plan { get { return HtmlSerializer.ToDecodedString(_plan); } set { _plan = value; } }
    }
}