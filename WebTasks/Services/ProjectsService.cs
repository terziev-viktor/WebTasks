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

        internal async Task<IEnumerable<ProjectVm>> GetUserProjectsToList(string filter, int page, string userId)
        {
            var a = await this.Context.Projects.Where(x => x.Creator.Id == userId && x.Title.Contains(filter))
                .OrderByDescending(x => x.Id)
                .Skip((page-1)*PageSize).Take(PageSize).ToListAsync();
            IEnumerable<ProjectVm> vm = Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(a);
            return vm;
        }

        internal async Task<ProjectDetailedAdminVm> GetProjectDetailedAdminVm(int? id)
        {
            Project p = await this.Context.Projects.FindAsync(id);
            p.Comments = p.Comments.OrderBy(x => x.PublishDate).ToList();
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
            vm.Comments = vm.Comments.OrderBy(x => x.PublishDate).ToList();
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

        internal async System.Threading.Tasks.Task Edit(ProjectBm bm)
        {
            Project p = await this.FindProjectAsync(bm.Id);
            p.Plan = bm.Plan;
            p.Description = bm.Description;
            p.ReleaseDate = bm.ReleaseDate;
            p.Title = bm.Title;

            await this.SaveChangesAsync();
        }
        
        internal void RemoveProject(Project project)
        {
            this.Context.Projects.Remove(project);
        }
    }
}