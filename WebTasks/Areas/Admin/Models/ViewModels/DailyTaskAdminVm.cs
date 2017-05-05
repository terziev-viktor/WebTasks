using WebTasks.Helpers;

namespace WebTasks.Areas.Admin.Models.ViewModels
{
    public class DailyTaskAdminVm
    {
        private string _title;
        private string _note;

        public int Id { get; set; }

        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }

        public string Note { get { return HtmlSerializer.ToDecodedString(_note); } set { _note = value; } }

        public int CommentsCount { get; set; }
        
        public string Creator { get; set; }

    }
}