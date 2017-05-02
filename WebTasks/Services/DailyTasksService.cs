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
using WebTasks.Models.Interfaces;
using WebTasks.Models;
using WebTasks.Services.Interfaces;
using System;

namespace WebTasks.Services
{
    public class DailyTasksService : Service, IDailyTasksService
    {
        public DailyTasksService(IApplicationDbContext context)
            : base(context, 10)
        {

        }

        public DailyTaskDetailedAdminVm GetDetailedDailyTaskAdminVmAsync(int? id)
        {
            var dt = this.Context.DailyTasks.Find(id);
            DailyTaskDetailedAdminVm vm = Mapper.Map<DailyTaskDetailedAdminVm>(dt);
            vm.Comments = vm.Comments.OrderByDescending(x => x.PublishDate).ToList();
            return vm;
        }

        public async Task<IEnumerable<DailyTaskAdminVm>> GetAllDailyTaskVm(string filter, int page)
        {
            var dt = await this.Context.DailyTasks.Where(x => x.Title.Contains(filter)).ToListAsync();

            var adminVm = Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskAdminVm>>(dt)
                .OrderByDescending(x => x.Id)
                .Skip(((page - 1) * PageSize)).Take(PageSize);
            
            return adminVm;
            
        }

        public async Task<IEnumerable<DailyTaskVm>> GetUserDailyTasksVm(string filter, int page, ApplicationUser currentUser)
        {
            var tasks = await this.Context.DailyTasks.Where(x => x.Creator.Id == currentUser.Id && x.Title.Contains(filter)).ToListAsync();
            IEnumerable<DailyTaskVm> tasksVm = Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskVm>>(tasks)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize).Take(PageSize);
            

            return tasksVm;
        }

        public async Task<IEnumerable<DailyTaskVm>> GetUserDailyTasks(ApplicationUser user)
        {
            var dailyTasks = await this.Context.DailyTasks.Where(x => x.Creator.Id == user.Id).ToListAsync();
            return Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskVm>>(dailyTasks);
        }

        public DailyTask FindById(int? id)
        {
            return this.Context.DailyTasks.Find(id);
        }
        
        public DailyTaskDetailedUserVm GetDetailedDailyTaskVm(int? id)
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

        public async System.Threading.Tasks.Task<int> CreateFromBm(DailyTaskBm bm, ApplicationUser creator)
        {
            var dailyTask = Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);
            dailyTask.Creator = creator;
            this.Context.DailyTasks.Add(dailyTask);
            return await this.Context.SaveChangesAsync();
        }

        public bool IsOwner(int id, ApplicationUser user)
        {
            return this.Context.DailyTasks.Find(id).Creator == user;
        }

        public Task<DailyTask> FindByIdAsync(int? id)
        {
            return this.Context.DailyTasks.FindAsync(id);
        }
        
        public async System.Threading.Tasks.Task Remove(DailyTask dailyTask)
        {
            this.Context.DailyTasks.Remove(dailyTask);
            await this.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Edit(DailyTaskBm bm)
        {
            DailyTask dt = await this.Context.DailyTasks.FindAsync(bm.Id);
            dt.Title = bm.Title;
            if(bm.EditedDeadline != null)
            dt.Deadline = (DateTime) bm.EditedDeadline;
            dt.Description = bm.Description;
            dt.Note = bm.Note;
            
            await this.SaveChangesAsync();
        }

        public DailyTask Map(DailyTaskBm bm)
        {
            return Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);

        }

        public int GetUserDailyTasksCount(string userId)
        {
            return this.Context.DailyTasks.Count(x => x.Creator.Id == userId);
        }
    }
}