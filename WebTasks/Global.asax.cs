using AutoMapper;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using System.Linq;
using WebTasks.Models;
using System.Web;
using WebTasks.Controllers;

namespace WebTasks
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            IController errorsController = new ErrorsController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }
    }

}
