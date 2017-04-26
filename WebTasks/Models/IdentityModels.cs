using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models.EntityModels;

namespace WebTasks.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("WebTasks")
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<DailyTask> DailyTasks { get; set; }

        public virtual DbSet<Project> Projects { get; set; }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyTask>().HasMany(x => x.Comments).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<Project>().HasMany(x => x.Comments).WithOptional().WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>().HasOptional<DailyTask>(x => x.DailyTask).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasOptional<Project>(x => x.Project).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}