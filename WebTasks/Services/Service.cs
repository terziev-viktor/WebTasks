using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using System.Linq;
using System;

namespace WebTasks.Services
{
    public class Service : IDisposable
    {
        private ApplicationDbContext context;

        private ApplicationUserManager userManager;

        protected Service()
        {
            this.context = new ApplicationDbContext();
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.context));
            this.InitAutoMapper();
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

        public void Dispose()
        {
            this.Context.Dispose();
        }

        private void InitAutoMapper()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Comment, CommentVm>().AfterMap((a, b) =>
                {
                    b.Author = a.Author.Email;
                });

                x.CreateMap<DailyTask, DailyTaskDetailedUserVm>()
                    .AfterMap((a, b) =>
                    {
                        b.CommentsCount = a.Comments.Count();
                        b.Creator_Name = a.Creator.UserName;
                    });

                x.CreateMap<DailyTask, DailyTaskVm>();

                x.CreateMap<DailyTaskBm, DailyTask>();

                x.CreateMap<Comment, CommentVm>().AfterMap((a, b) =>
                {
                    b.Author = a.Author.UserName;
                });
                x.CreateMap<DailyTask, DailyTaskAdimVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                    b.Creator = a.Creator.UserName;
                });
                x.CreateMap<DailyTask, DailyTaskDetailedAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                    b.Creator = a.Creator.UserName;
                });

                x.CreateMap<DailyTask, DailyTaskVm>();
                x.CreateMap<Project, ProjectVm>();

                x.CreateMap<ApplicationUser, PersonVm>().AfterMap((a, b) =>
                {
                    b.DailyTasksCount = this.Context.DailyTasks.Where(dt => dt.Creator.Id == a.Id).Count();
                    b.ProjectsCount = this.Context.Projects.Where(dt => dt.Creator.Id == a.Id).Count();
                    b.TasksCount = b.DailyTasksCount + b.ProjectsCount;
                });

                x.CreateMap<ApplicationUser, PersonDetailedVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = this.Context.Comments.Where(c => c.Author.UserName == a.UserName).Count();
                });

                x.CreateMap<Project, ProjectDetailedUserVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                });

                x.CreateMap<Project, ProjectVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                });

                x.CreateMap<ProjectBm, Project>();
                x.CreateMap<Project, ProjectAdminVm>().AfterMap((a, b) =>
                {
                    b.Creator = a.Creator.UserName;
                    b.CommentsCount = a.Comments.Count();
                });
                x.CreateMap<Project, ProjectDetailedAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                    b.Creator = a.Creator.UserName;
                });

                x.CreateMap<ApplicationUser, PersonDetailedAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = this.context.Comments.Count(c => c.Author.Id == b.Id);
                });

                x.CreateMap<ApplicationUser, PersonAdminVm>().AfterMap((a, b) =>
                {
                    b.ProjectsCount = this.context.Projects.Count(c => c.Creator.Id == a.Id);
                    b.DailyTasksCount = this.context.DailyTasks.Count(c => c.Creator.Id == a.Id);
                });
            });
        }
    }
}