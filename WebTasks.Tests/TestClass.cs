using System;
using WebTasks.Mocks;
using WebTasks.Models;
using WebTasks.Models.EntityModels;
using WebTasks.Models.Interfaces;

namespace WebTasks.Tests
{
    public class TestClass
    {
        protected IApplicationDbContext context;
        protected ApplicationUser UserA = new ApplicationUser()
        {
            Id = "radnomstring",
            Email = "Some@email.com",
            UserName = "Some@email.com"
        };
        protected ApplicationUser UserB = new ApplicationUser()
        {
            Id = "someotherradnomstring",
            Email = "other@email.com",
            UserName = "other@email.com"
        };

        public void InitContext()
        {
            context = new MockedDbContext();
            
            context.Projects.Add(new Project()
            {
                Id = 1,
                Plan = "inir 1",
                Description = "dsgdsgdsgestesa",
                ReleaseDate = DateTime.Now,
                Title = "ndfjksdfn",
                Creator = UserA
            });

            context.DailyTasks.Add(new DailyTask()
            {
                Deadline = DateTime.Now,
                Description = "Init dt 1",
                Note = "Some shit",
                Title = "Init 1",
                Id = 1,
                Creator = UserA

            });

            context.Comments.Add(new Comment()
            {
                Content = "Init 1",
                Id = 1,
                PublishDate = DateTime.Now,
                Author = UserB,
                DailyTaskId = 1
            });

            context.Comments.Add(new Comment()
            {
                Content = "Init 1",
                Id = 2,
                PublishDate = DateTime.Now,
                Author = UserB,
                Project_Id = 1
            });
        }
    }
}
