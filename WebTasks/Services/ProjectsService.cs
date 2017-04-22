using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using WebTasks.Models.EntityModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Services
{
    public class ProjectsService : Service
    {
        public ProjectsService()
            : base()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Project, ProjectDetailedVm>().AfterMap((a, b) =>
                {
                    b.Creator = a.Creator.UserName;
                    b.CommentsCount = a.Comments.Count();
                });

                x.CreateMap<Project, ProjectVm>().AfterMap((a, b) =>
                {
                    b.CommentsCount = a.Comments.Count();
                });
            });
        }

        internal IEnumerable<Project> GetProjectsToList()
        {
            return Context.Projects.ToList();
        }

        internal Task<Project> FindProjectAsync(int? id)
        {
            return this.Context.Projects.FindAsync(id);
        }

        internal ProjectDetailedVm GetDetailedVm(Project p)
        {
            return Mapper.Map<ProjectDetailedVm>(p);
        }

        internal void AddProject(ProjectBm bm)
        {
            Project p = Mapper.Instance.Map<ProjectBm, Project>(bm);
            this.Context.Projects.Add(p);
            this.Context.SaveChanges();
        }

        internal ProjectVm GetProjectVm(Project project)
        {
            return Mapper.Map<ProjectVm>(project);
        }

        internal void UpdateProject(Project p, DateTime releaseDate, string plan, string title, string description)
        {
            p.ReleaseDate = releaseDate;
            p.Description = description;
            p.Plan = plan;
            p.Title = title;
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