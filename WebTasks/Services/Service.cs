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
using System.Data.Entity.Validation;

namespace WebTasks.Services
{
    public class Service : IDisposable
    {
        private ApplicationDbContext context;

        private ApplicationUserManager userManager;

        protected Service(int pageSize)
        {
            this.PageSize = pageSize;
            this.context = new ApplicationDbContext();
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.context));
            this.InitAutoMapper();
        }

        protected ApplicationDbContext Context => this.context;

        protected ApplicationUserManager UserManager => this.userManager;

        protected int PageSize { get; set; }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            try
            {
                int x = await this.Context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e.EntityValidationErrors);
                throw;
            }
        }

        public void SaveChanges()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                
                x.CreateMap<DailyTask, DailyTaskAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                    b.Creator = a.Creator.UserName;
                });
                x.CreateMap<DailyTask, DailyTaskDetailedAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                    b.Creator = a.Creator.UserName;
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

                // Application User Vm
                x.CreateMap<ApplicationUser, PersonDetailedAdminVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = this.context.Comments.Count(c => c.Author.Id == b.Id);
                });

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
            });
        }
    }
}