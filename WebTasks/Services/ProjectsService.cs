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
using WebTasks.Models.Interfaces;
using WebTasks.Models;
using WebTasks.Services.Interfaces;
using System;

namespace WebTasks.Services
{
    public class ProjectsService : Service, IProjectsService
    {
        public ProjectsService(IApplicationDbContext context)
            : base(context, 10)
        {

        }

        public IEnumerable<ProjectVm> GetUserProjectsVm(string id)
        {
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(this.Context.Projects.Where(x => x.Creator.Id == id));
        }

        public async Task<IEnumerable<ProjectVm>> GetUserProjectsToList(string filter, int page, ApplicationUser user)
        {
            var a = await this.Context.Projects.Where(x => x.Creator.Id == user.Id && x.Title.Contains(filter))
                .OrderByDescending(x => x.Id)
                .Skip((page-1)*PageSize).Take(PageSize).ToListAsync();
            IEnumerable<ProjectVm> vm = Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(a);
            return vm;
        }

        public async Task<ProjectDetailedAdminVm> GetProjectDetailedAdminVm(int? id)
        {
            Project p = await this.Context.Projects.FindAsync(id);
            p.Comments = p.Comments.OrderBy(x => x.PublishDate).ToList();
            return Mapper.Map<ProjectDetailedAdminVm>(p);
        }

        public IEnumerable<ProjectAdminVm> MapToProjectsAdminVm(IEnumerable<Project> p)
        {
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectAdminVm>>(p);
        }

        public async Task<IEnumerable<ProjectVm>> GetUserProjects(string id)
        {
            var projects = await this.Context.Projects.Where(x => x.Creator.Id == id).ToListAsync();
            return Mapper.Instance.Map<IEnumerable<Project>, IEnumerable<ProjectVm>>(projects);
        }

        public async Task<IEnumerable<Project>> GetProjectsToListAsync(string filter, int page)
        {
            return await this.Context.Projects
                .Where(x => x.Title.Contains(filter))
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(ProjectBm bm, ApplicationUser creator)
        {
            Project p = Mapper.Map<Project>(bm);
            p.Creator = creator;
            this.Context.Projects.Add(p);
            await this.Context.SaveChangesAsync();
        }

        public ProjectAdminVm MapToProjectAdminVm(Project project)
        {
            return Mapper.Map<ProjectAdminVm>(project);
        }

        public IEnumerable<Project> GetProjectsToList()
        {
            return Context.Projects.ToList();
        }

        public Task<Project> FindAsync(int? id)
        {
            return this.Context.Projects.FindAsync(id);
        }

        public ProjectDetailedUserVm GetDetailedVm(Project p)
        {
            ProjectDetailedUserVm vm = Mapper.Map<ProjectDetailedUserVm>(p);
            vm.Comments = vm.Comments.OrderBy(x => x.PublishDate).ToList();
            return vm;
        }

        public ProjectVm GetProjectVm(Project project)
        {
            return Mapper.Map<ProjectVm>(project);
        }

        public async System.Threading.Tasks.Task Edit(ProjectBm bm)
        {
            Project p = await this.FindAsync(bm.Id);
            p.Plan = bm.Plan;
            p.Description = bm.Description;
            if(bm.EditedReleaseDate != null)
            p.ReleaseDate = (DateTime) bm.EditedReleaseDate;
            p.Title = bm.Title;

            await this.SaveChangesAsync();
        }
        
        public async System.Threading.Tasks.Task RemoveAsync(Project project)
        {
            this.Context.Projects.Remove(project);
            await this.Context.SaveChangesAsync();
        }

        public int GetUserProjectsCount(string userId)
        {
            return this.Context.Projects.Count(x => x.Creator.Id == userId);
        }
    }
}