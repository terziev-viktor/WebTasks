using System;
using System.Collections.Generic;
using WebTasks.Helpers;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.User.Models.ViewModels
{
    public class DailyTaskDetailedUserVm
    {
        private string _desc;
        private string _title;
        private string _note;

        public DailyTaskDetailedUserVm()
        {
            this.Comments = new List<CommentVm>();
        }

        public int Id { get; set; }
        
        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }


        public string Note { get { return HtmlSerializer.ToDecodedString(_note); } set { _note = value; } }

        public string Description { get { return HtmlSerializer.ToDecodedString(_desc); } set { _desc = value; } }

        public DateTime Deadline { get; set; }
        
        public int CommentsCount { get; set; }

        public bool IsOwner { get; set; }

        public List<CommentVm> Comments;
    }
}