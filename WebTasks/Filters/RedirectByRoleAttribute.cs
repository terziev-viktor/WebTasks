using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebTasks.Utils;

namespace WebTasks.Filters
{
    public class RedirectByRoleAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuted(HttpActionExecutedContext ctx)
        //{
        //    if (HttpContext.Current.User.IsInRole("Admin"))
        //    {
        //        // redirect to area admin
        //        HttpContext.Current.Response.Redirect(StaticUrl.AdminHomePage);
        //    }

        //    if (HttpContext.Current.User.IsInRole("User"))
        //    {
        //        // redirect to user area
        //        HttpContext.Current.Response.Redirect(StaticUrl.UserHomePage);
        //    }

        //    base.OnActionExecuted(ctx);
        //}

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                // redirect to area admin
                HttpContext.Current.Response.Redirect(StaticUrl.AdminHomePage);
            }

            if (HttpContext.Current.User.IsInRole("User"))
            {
                // redirect to user area
                HttpContext.Current.Response.Redirect(StaticUrl.UserHomePage);
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override void OnActionExecuting(HttpActionContext ctx)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return;
            }
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                // redirect to area admin
                HttpContext.Current.Response.Redirect(StaticUrl.AdminHomePage);
            }

            if (HttpContext.Current.User.IsInRole("User"))
            {
                // redirect to user area
                HttpContext.Current.Response.Redirect(StaticUrl.UserHomePage);
            }

            base.OnActionExecuting(ctx);
        }
    }
}