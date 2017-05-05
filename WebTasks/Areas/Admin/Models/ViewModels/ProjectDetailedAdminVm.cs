﻿using System;
using System.Collections.Generic;
using WebTasks.Helpers;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class ProjectDetailedAdminVm
    {
        private string _title;
        private string _desc;
        private string _plan;

        public int Id { get; set; }

        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }

        public string Description { get { return HtmlSerializer.ToDecodedString(_desc); } set { _desc = value; } }

        public string Creator { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Plan { get { return HtmlSerializer.ToDecodedString(_plan); } set { _plan = value; } }

        public int CommentsCount { get; set; }

        public IEnumerable<CommentVm> Comments { get; set; }
    }
}