using System;
using System.Web;

namespace WebTasks.Helpers
{
    public class HtmlSerializer
    {
        public static string ToEncodedString(string str)
        {
            if (str == null)
            {
                return str;
            }
            return HttpContext.Current.Server.HtmlEncode(str).Trim();
        }

        internal static string ToDecodedString(string str)
        {
            if(str == null)
            {
                return str;
            }
            return HttpContext.Current.Server.HtmlDecode(str).Trim();
        }
    }
}