using System;
using System.Collections.Generic;
using WebTasks.Helpers;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.User.Models.ViewModels
{
    public class ProjectDetailedUserVm
    {
        private string _desc;
        private string _title;
        private string _plan;
        public int Id { get; set; }
        
        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }
        
        public string Description { get { return HtmlSerializer.ToDecodedString(_desc); } set { _desc = value; } }
        
        public DateTime ReleaseDate { get; set; }
        
        public string Plan { get { return HtmlSerializer.ToDecodedString(_plan); } set { _plan = value; } }

        public bool IsOwner { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<CommentVm> Comments { get; set; }
    }
}