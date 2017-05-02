using System.Web.Mvc;

namespace WebTasks.Helpers
{
    public class HtmlSerializer
    {
        public static string ToHtmlString(string str)
        {
            return MvcHtmlString.Create(str).ToHtmlString().Trim();
        }
    }
}