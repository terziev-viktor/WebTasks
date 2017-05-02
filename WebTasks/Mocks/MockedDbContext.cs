using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebTasks.Models.EntityModels;
using WebTasks.Models.Interfaces;

namespace WebTasks.Mocks
{
    public class MockedDbContext : IApplicationDbContext
    {
        public MockedDbContext()
        {
            Comments = new MockedCommentsDbSet();
            DailyTasks = new MockedDailyTasksDbSet();
            Projects = new MockedProjectsDbSet();
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<DailyTask> DailyTasks { get; set; }

        public DbSet<Project> Projects { get; set; }
        

        public int SaveChanges()
        {
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return new Task<int>(() => 1);
        }
    }
}