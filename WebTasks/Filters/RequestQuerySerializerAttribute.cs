using System.Web.Http.Filters;
using System.Web.Mvc;

namespace WebTasks.Filters
{
    public class RequestQuerySerializerAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var query in filterContext.HttpContext.Request.QueryString)
            {
                string str = new MvcHtmlString(query.ToString()).ToHtmlString();
                filterContext.HttpContext.Request.QueryString[query as string] = str;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}