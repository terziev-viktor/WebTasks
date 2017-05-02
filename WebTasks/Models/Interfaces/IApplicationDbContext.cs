using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebTasks.Models.EntityModels;

namespace WebTasks.Models.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Comment> Comments { get; set; }

        DbSet<DailyTask> DailyTasks { get; set; }

        DbSet<Project> Projects { get; set; }

        int SaveChanges();

        System.Threading.Tasks.Task<int> SaveChangesAsync();
    }
}