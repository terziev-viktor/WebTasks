using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using WebTasks.Models;

namespace WebTasks.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<WebTasks.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "WebTasks.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // add role admin and user, add 2 default users, set roles to default users
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var newAdmin = new ApplicationUser()
            {
                Email = "the@admin.net",
                UserName = "the@admin.net"
            };
            var newUser = new ApplicationUser()
            {
                Email = "user@user.net",
                UserName = "user@user.net"
            };

            var adminCreated = userManager.Create(newAdmin, "S!mepass12345");
            var userCreated = userManager.Create(newUser, "S!mepass12345");

            if (!roleManager.Roles.Any())
            {
                var roleAdminCreated = roleManager.Create(new IdentityRole("Admin"));
                var roleUserCreated = roleManager.Create(new IdentityRole("User"));
            }

            var userA= userManager.Users.FirstOrDefault(x => x.Email == "the@admin.net");
            userManager.AddToRole(userA.Id, "Admin");

            var userU = userManager.Users.FirstOrDefault(x => x.Email == "user@user.net");
            userManager.AddToRole(userU.Id, "User");

            var dtask1 = new Models.EntityModels.DailyTask()
            {
                Title = "Task1",
                Note = "this is the first task",
                Description = "Has no desc",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            };
            dtask1.Comments.Add(new Models.EntityModels.Comment()
            {
                Author = userManager.Users.First(),
                PublishDate = DateTime.Now,
                Content = "In up so discovery my " +
                "middleton eagerness dejection explained. " +
                "Estimating excellence ye contrasted insensible as. " +
                "Oh up unsatiable advantages decisively as at interested. Present suppose in esteems in demesne colonel it to. " +
                "End horrible she landlord screened stanhill. Repeated offended you opinions off dissuade ask packages screened. ",
            });
            dtask1.Comments.Add(new Models.EntityModels.Comment()
            {
                Author = userManager.Users.First(),
                PublishDate = DateTime.Now,
                Content = "In up so discovery my " +
                "middleton eagerness dejection explained. " +
                "Estimating excellence ye contrasted insensible as. " +
                "Oh up unsatiable advantages decisively as at interested. Present suppose in esteems in demesne colonel it to. " +
                "End horrible she landlord screened stanhill. Repeated offended you opinions off dissuade ask packages screened. ",
            });
            dtask1.Comments.Add(new Models.EntityModels.Comment()
            {
                Author = userManager.Users.First(),
                PublishDate = DateTime.Now,
                Content = "In up so discovery my " +
                "middleton eagerness dejection explained. " +
                "Estimating excellence ye contrasted insensible as. " +
                "Oh up unsatiable advantages decisively as at interested. Present suppose in esteems in demesne colonel it to. " +
                "End horrible she landlord screened stanhill. Repeated offended you opinions off dissuade ask packages screened. ",
            });

            context.DailyTasks.Add(dtask1);

            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task2",
                Note = "this is the second task",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });
            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task3",
                Note = "this is the third task",
                Description = "Has no desc",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });
            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task4",
                Note = "this is the task",
                Description = "Has no desc",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });
            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task5",
                Note = "something",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });
            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task6",
                Note = "this is a task",
                Description = "Something interesting",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });
            context.DailyTasks.Add(new Models.EntityModels.DailyTask()
            {
                Title = "Task7",
                Deadline = DateTime.Now,
                Creator = context.Users.First(x => x.UserName == "user@user.net")
            });

            context.SaveChanges();
        }
    }
}
