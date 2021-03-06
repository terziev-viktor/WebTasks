﻿using System.Web.Mvc;
using WebTasks.Filters;
using WebTasks.Enums;

namespace WebTasks.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ActionResult RedirectByRole()
        {
            if (this.User.IsInRole("Admin"))
                return Redirect(StaticUrl.AdminHomePage);
            if (this.User.IsInRole("User"))
                return Redirect(StaticUrl.UserHomePage);
            return null;
        }

        public ActionResult Index()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectByRole();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "WebTasks";

            return View();
        }
        [RedirectByRole]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}