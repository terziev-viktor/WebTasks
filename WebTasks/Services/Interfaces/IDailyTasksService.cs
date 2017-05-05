using System.Collections.Generic;
using System.Threading.Tasks;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services.Interfaces
{
    public interface IDailyTasksService
    {
        DailyTaskDetailedAdminVm GetDetailedDailyTaskAdminVmAsync(int? id);

        Task<IEnumerable<DailyTaskAdminVm>> GetAllDailyTaskVm(string filter, int page);

        Task<IEnumerable<DailyTaskVm>> GetUserDailyTasksVm(string filter, int page, ApplicationUser user);

        Task<IEnumerable<DailyTaskVm>> GetUserDailyTasks(ApplicationUser user);

        DailyTask FindById(int? id);

        DailyTaskDetailedUserVm GetDetailedDailyTaskVm(int? id, string currentUserId);

        Task<int> Create(DailyTaskBm bm, ApplicationUser creator);

        bool IsOwner(int id, ApplicationUser user);

        Task<DailyTask> FindByIdAsync(int? id);

        System.Threading.Tasks.Task Remove(DailyTask dailyTask);

        System.Threading.Tasks.Task Edit(DailyTaskBm bm);

        DailyTask Map(DailyTaskBm bm);

        Models.Interfaces.IApplicationDbContext Context { get; set; }

        int GetUserDailyTasksCount(string v);
    }
}