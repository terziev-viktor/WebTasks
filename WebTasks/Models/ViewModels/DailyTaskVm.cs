using WebTasks.Helpers;

namespace WebTasks.Models.ViewModels
{
    public class DailyTaskVm
    {
        private string _title;
        private string _note;

        public int Id { get; set; }

        public string Title { get { return HtmlSerializer.ToDecodedString(_title); } set { _title = value; } }

        public string Note { get { return HtmlSerializer.ToEncodedString(_note); } set { _note = value; } }

        public int CommentsCount { get; set; }
    }
}