using Microsoft.AspNet.Identity.EntityFramework;
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

        public System.Threading.Tasks.Task SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void DisposeContext()
        {
            this.Context.Dispose();
        }
    }
}