using System;
using System.ComponentModel.DataAnnotations;
using WebTasks.Helpers;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class ProjectAdminVm
    {
        private string _desc;
        private string _plan;
        private string _title;

        public int Id { get; set; }

        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }

        public string Description { get { return HtmlSerializer.ToDecodedString(_desc); } set { _desc = value; } }

        public int CommentsCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Plan { get { return HtmlSerializer.ToDecodedString(_plan); } set { _plan = value; } }
        
        public string Creator { get; set; }
    }
}