using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Models.BindingModels;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebTasks.Services
{
    public class DailyTasksService : Service
    {
        public DailyTasksService()
            : base()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<DailyTask, DailyTaskDetailedVm>()
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
            });
        }

        internal IEnumerable<DailyTaskVm> GetUserDailyTasksVm(string name)
        {
            var currentUser = this.UserManager.Users.First(x => x.UserName == name);
            var tasks = this.Context.DailyTasks.Where(x => x.Creator.Id == currentUser.Id).ToList();
            IEnumerable<DailyTaskVm> tasksVm = Mapper.Instance.Map<IEnumerable<DailyTask>, IEnumerable<DailyTaskVm>>(tasks);

            return tasksVm;
        }

        internal DailyTask GetDailyTaskById(int? id)
        {
            return this.Context.DailyTasks.Find(id);
        }
        
        internal DailyTaskDetailedVm GetDetailedDailyTaskVm(int? id)
        {
            DailyTask dailyTask = this.Context.DailyTasks.Find(id);
            
            if (dailyTask == null)
            {
                return null;
            }
            DailyTaskDetailedVm vm = Mapper.Map<DailyTaskDetailedVm>(dailyTask);

            return vm;
        }

        internal async System.Threading.Tasks.Task<int> CreateNewTask(DailyTaskBm bm, string name)
        {
            var dailyTask = Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);

            var creator = this.UserManager.Users.FirstOrDefault(x => x.UserName == name);
            dailyTask.Creator = creator;
            this.Context.DailyTasks.Add(dailyTask);
            return await this.Context.SaveChangesAsync();
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

        internal async System.Threading.Tasks.Task Edit(int id, string note, string title, string description, DateTime deadline)
        {
            DailyTask dt = this.Context.DailyTasks.Find(id);
            dt.Title = title;
            dt.Description = description;
            dt.Note = dt.Note;
            dt.Deadline = deadline;
            await this.SaveChangesAsync();
        }

        internal DailyTask GetDailyTask(DailyTaskBm bm)
        {
            return Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);

        }
    }
}