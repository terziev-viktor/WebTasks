using System;
using System.Collections.Generic;
using WebTasks.Helpers;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class DailyTaskDetailedAdminVm
    {
        private string _desc;
        private string _note;
        private string _title;
        public DailyTaskDetailedAdminVm()
        {
            this.Comments = new List<CommentVm>();
        }

        public int Id { get; set; }

        
        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }
        
        public string Description
        {
            get
            {
                return HtmlSerializer.ToDecodedString(_desc);
            }
            set { _desc = value; }
        }
        
        public string Creator { get; set; }

        public ICollection<CommentVm> Comments { get; set; }

        public int CommentsCount { get; set; }

        public string Note { get { return HtmlSerializer.ToDecodedString(_note); } set { _note = value; } }
        
        public DateTime Deadline { get; set; }
    }
}