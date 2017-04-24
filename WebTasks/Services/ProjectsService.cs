using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using WebTasks.Models.EntityModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using WebTasks.Areas.Admin.Models.ViewModels;

namespace WebTasks.Services
{
    public class ProjectsService : Service
    {
        public ProjectsService()
            : base()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Comment, CommentVm>();
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
            });
        }

        internal async Task<ProjectDetailedAdminVm> GetProjectDetailedAdminVm(int? id)
        {
            Project p = await this.Context.Projects.FindAsync(id);
            return Mapper.Map<ProjectDetailedAdminVm>(p);
        }

        internal IEnumerable<ProjectAdminVm> MapToProjectsAdminVm(IEnumerable<Project> p)
        {
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectAdminVm>>(p);
        }

        internal async Task<IEnumerable<Project>> GetProjectsToListAsync()
        {
            return await this.Context.Projects.ToListAsync();
        }

        internal async System.Threading.Tasks.Task AddProjectAsync(ProjectBm bm, string creator)
        {
            Project p = Mapper.Map<Project>(bm);
            p.Creator = this.UserManager.Users.First(x => x.UserName == creator);
            this.Context.Projects.Add(p);
            await this.Context.SaveChangesAsync();
        }

        internal ProjectAdminVm MapToProjectAdminVm(Project project)
        {
            return Mapper.Map<ProjectAdminVm>(project);
        }

        internal IEnumerable<Project> GetProjectsToList()
        {
            return Context.Projects.ToList();
        }

        internal Task<Project> FindProjectAsync(int? id)
        {
            return this.Context.Projects.FindAsync(id);
        }

        internal ProjectDetailedUserVm GetDetailedVm(Project p)
        {
            ProjectDetailedUserVm vm = Mapper.Map<ProjectDetailedUserVm>(p);
            return vm;
        }

        internal void AddProject(ProjectBm bm, string creator)
        {
            Project p = Mapper.Map<Project>(bm);
            p.Creator = this.UserManager.Users.First(x => x.UserName == creator);
            this.Context.Projects.Add(p);
        }

        internal ProjectVm GetProjectVm(Project project)
        {
            return Mapper.Map<ProjectVm>(project);
        }
        
        internal void UpdateProject(Project p)
        {
            this.Context.Entry(p).State = EntityState.Modified;
            this.Context.SaveChanges();
        }

        internal void SetEntryState(Project p, EntityState state)
        {
            this.Context.Entry(p).State = state;
        }

        internal void RemoveProject(Project project)
        {
            this.Context.Projects.Remove(project);
        }
    }
}