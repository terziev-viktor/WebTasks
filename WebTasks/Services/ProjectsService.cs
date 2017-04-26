using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using WebTasks.Models.EntityModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using WebTasks.Areas.Admin.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace WebTasks.Services
{
    public class ProjectsService : Service
    {
        public ProjectsService()
            : base(10)
        {

        }

        internal IEnumerable<ProjectVm> GetUserProjectsVm(string id)
        {
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(this.Context.Projects.Where(x => x.Creator.Id == id));
        }

        internal async Task<IEnumerable<Project>> GetUserProjectsToList(string filter, int page, string userId)
        {
            return await this.Context.Projects.Where(x => x.Creator.Id == userId && x.Title.Contains(filter))
                .OrderByDescending(x => x.Id)
                .Skip((page-1)*PageSize).Take(PageSize).ToListAsync();
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

        internal async Task<IEnumerable<ProjectVm>> GetUserProjects(string id)
        {
            var projects = await this.Context.Projects.Where(x => x.Creator.Id == id).ToListAsync();
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(projects);
        }

        internal async Task<IEnumerable<Project>> GetProjectsToListAsync(string filter, int page)
        {
            return await this.Context.Projects
                .Where(x => x.Title.Contains(filter))
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
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
            p.Creator = this.UserManager.FindById(creator);

            this.Context.Projects.Add(p);
        }

        internal ProjectVm GetProjectVm(Project project)
        {
            return Mapper.Map<ProjectVm>(project);
        }

        internal void UpdateProject(Project p, ProjectBm bm)
        {
            p.Plan = bm.Plan;
            p.Description = bm.Description;
            p.ReleaseDate = bm.ReleaseDate;
            p.Title = bm.Title;
            this.Context.SaveChanges();
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