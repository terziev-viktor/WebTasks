using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebTasks.Models;

namespace WebTasks.Services
{
    public class Service
    {
        private ApplicationDbContext context;
        private ApplicationUserManager userManager;
        protected Service()
        {
            this.context = new ApplicationDbContext();
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.context));
        }

        protected ApplicationDbContext Context => this.context;

        protected ApplicationUserManager UserManager => this.userManager;
    }
}