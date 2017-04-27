using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Models.BindingModels;
using System.Threading.Tasks;
using System.Data.Entity;
using WebTasks.Areas.Admin.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace WebTasks.Services
{
    public class DailyTasksService : Service
    {
        public DailyTasksService()
            : base(10)
        {

        }

        internal DailyTaskDetailedAdminVm GetDetailedDailyTaskAdminVmAsync(int? id)
        {
            var dt = this.Context.DailyTasks.Find(id);
            DailyTaskDetailedAdminVm vm = Mapper.Map<DailyTaskDetailedAdminVm>(dt);
            vm.Comments = vm.Comments.OrderByDescending(x => x.PublishDate).ToList();
            return vm;
        }

        internal async Task<IEnumerable<DailyTaskAdminVm>> GetAllDailyTaskVm(string filter, int page)
        {
            var dt = await this.Context.DailyTasks.Where(x => x.Title.Contains(filter)).ToListAsync();

            var adminVm = Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskAdminVm>>(dt)
                .OrderByDescending(x => x.Id)
                .Skip(((page - 1) * PageSize)).Take(PageSize);
            
            return adminVm;
            
        }

        internal async Task<IEnumerable<DailyTaskVm>> GetUserDailyTasksVm(string filter, int page, string id)
        {
            var currentUser = this.UserManager.FindById(id);

            var tasks = await this.Context.DailyTasks.Where(x => x.Creator.Id == currentUser.Id && x.Title.Contains(filter)).ToListAsync();
            IEnumerable<DailyTaskVm> tasksVm = Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskVm>>(tasks)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize).Take(PageSize);
            

            return tasksVm;
        }

        internal async Task<IEnumerable<DailyTaskVm>> GetUserDailyTasks(string id)
        {
            var dailyTasks = await this.Context.DailyTasks.Where(x => x.Creator.Id == id).ToListAsync();
            return Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskVm>>(dailyTasks);
        }

        internal DailyTask GetDailyTaskById(int? id)
        {
            return this.Context.DailyTasks.Find(id);
        }
        
        internal DailyTaskDetailedUserVm GetDetailedDailyTaskVm(int? id)
        {
            DailyTask dailyTask = this.Context.DailyTasks.Find(id);
            
            if (dailyTask == null)
            {
                return null;
            }
            DailyTaskDetailedUserVm vm = Mapper.Map<DailyTaskDetailedUserVm>(dailyTask);
            vm.Comments = vm.Comments.OrderBy(x => x.PublishDate).ToList();
            return vm;
        }

        internal async System.Threading.Tasks.Task<int> CreateNewTask(DailyTaskBm bm, string currentUserId)
        {
            var dailyTask = Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);

            var creator = await this.UserManager.FindByIdAsync(currentUserId);

            dailyTask.Creator = creator;
            this.Context.DailyTasks.Add(dailyTask);
            return await this.Context.SaveChangesAsync();
        }

        internal bool IsOwner(int id, string v)
        {
            return this.Context.DailyTasks.Find(id).Creator.Id == v;
        }

        internal Task<DailyTask> FindDailyTaskById(int? id)
        {
            return this.Context.DailyTasks.FindAsync(id);
        }

        internal void SetEntryState(DailyTask dt, EntityState m)
        {
            this.Context.Entry(dt).State = m;
        }
        
        internal void Remove(DailyTask dailyTask)
        {
            this.Context.DailyTasks.Remove(dailyTask);
        }

        internal async System.Threading.Tasks.Task Edit(DailyTaskBm bm)
        {
            DailyTask dt = await this.Context.DailyTasks.FindAsync(bm.Id);
            dt.Title = bm.Title;
            dt.Deadline = bm.Deadline;
            dt.Description = bm.Description;
            dt.Note = bm.Note;
            
            await this.SaveChangesAsync();
        }

        internal DailyTask GetDailyTask(DailyTaskBm bm)
        {
            return Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);

        }
    }
}