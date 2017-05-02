using System.Collections.Generic;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services.Interfaces
{
    public interface IProjectsService
    {
        IEnumerable<ProjectVm> GetUserProjectsVm(string id);

        System.Threading.Tasks.Task<IEnumerable<ProjectVm>> GetUserProjectsToList(string filter, int page, ApplicationUser user);

        System.Threading.Tasks.Task<ProjectDetailedAdminVm> GetProjectDetailedAdminVm(int? id);

        IEnumerable<ProjectAdminVm> MapToProjectsAdminVm(IEnumerable<Project> p);

        System.Threading.Tasks.Task<IEnumerable<ProjectVm>> GetUserProjects(string id);

        System.Threading.Tasks.Task<IEnumerable<Project>> GetProjectsToListAsync(string filter, int page);

        System.Threading.Tasks.Task AddAsync(ProjectBm bm, ApplicationUser creator);

        ProjectAdminVm MapToProjectAdminVm(Project project);

        IEnumerable<Project> GetProjectsToList();

        System.Threading.Tasks.Task<Project> FindAsync(int? id);

        ProjectDetailedUserVm GetDetailedVm(Project p);

        ProjectVm GetProjectVm(Project project);

        System.Threading.Tasks.Task Edit(ProjectBm bm);

        System.Threading.Tasks.Task RemoveAsync(Project project);

        Models.Interfaces.IApplicationDbContext Context { get; set; }

        int GetUserProjectsCount(string v);
    }
}