using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTasks.Models.BindingModels
{
    public class ProjectBm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime? EditedReleaseDate { get; set; }

        public string Description { get; set; }

        public string Plan { get; set; }
    }
}