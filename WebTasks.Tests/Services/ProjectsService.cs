using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Services;

namespace WebTasks.Tests.Services
{
    [TestClass]
    public class ProjectsServiceTests : TestClass
    {
        public ProjectsService service;

        [TestInitialize]
        public void Init()
        {
            InitContext();
            this.service = new ProjectsService(context);
            
        }

        [TestMethod]
        public void UserProjectsCount_ShouldReturnNumberOfCreatedProjectsByUser()
        {
            // arrange
            string id = UserA.Id;
            int expected = this.context.Projects.Count(x => x.Creator.Id == id);

            // act
            int result = this.service.GetUserProjectsCount(id);

            // assert
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        public void GetDetailedAdminVm_ShouldMapToDetailedAdminVm()
        {
            // arrange
            var project = this.context.Projects.First();

            // act
            var result = service.GetProjectDetailedAdminVm(project.Id).Result;

            // assert
            Assert.IsInstanceOfType(result, typeof(ProjectDetailedAdminVm));
        }

        [TestMethod]
        public void GetDetailedVm_ShouldMapToDetailedVm()
        {
            // arrange
            var project = this.context.Projects.First();

            // act
            var result = this.service.GetDetailedVm(project);

            // assert
            Assert.IsInstanceOfType(result, typeof(ProjectDetailedUserVm));
            Assert.AreEqual(project.Id, result.Id);
            Assert.AreEqual(project.Plan, result.Plan);
            Assert.AreEqual(project.ReleaseDate, result.ReleaseDate);
        }

        [TestMethod]
        public void GetProjectsToList_ShouldReturnListOfAllProjects()
        {
            // arrange 
            var expected = this.context.Projects.ToList();

            // act
            var result = this.service.GetProjectsToList();

            // assert
            Assert.AreEqual(expected.Count, result.Count());
        }

        [TestMethod]
        public void GetProjectVm_ShouldReturnViewModelOfTask()
        {
            // arrange
            Project p = this.context.Projects.First();

            // act
            var result = this.service.GetProjectVm(p);

            // assert
            Assert.IsInstanceOfType(result, typeof(ProjectVm));
            Assert.AreEqual(p.Id, result.Id);
            Assert.AreEqual(p.Description, result.Description);
            Assert.AreEqual(p.Plan, result.Plan);
        }

        [TestMethod]
        public void MapToProjectAdminVm_ShouldMapProject()
        {
            // arrange
            Project expected = this.context.Projects.First();

            // act
            var result = this.service.MapToProjectAdminVm(expected);

            // assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Plan, result.Plan);
            Assert.AreEqual(expected.ReleaseDate, result.ReleaseDate);
            Assert.AreEqual(expected.Title, result.Title);
            Assert.AreEqual(expected.Description, result.Description);

        }

        [TestMethod]
        public void MapToProjectsAdminVm_ShouldMapProjects()
        {
            // arrange
            var projects = this.context.Projects.ToList();

            // act
            var result = this.service.MapToProjectsAdminVm(projects);

            // assert
            Assert.AreEqual(projects.Count, result.Count());
        }

        [TestMethod]
        public void GetUserProjectsVm_ShoidReturnUsersProjectsAsViewModels()
        {
            // arrange
            string id = UserA.Id;
            var expected = this.context.Projects.Count(x => x.Creator.Id == UserA.Id);

            // act
            var result = this.service.GetUserProjectsVm(id);

            // assert
            Assert.AreEqual(expected, result.Count());
        }

        [TestMethod]
        public void AddAsync_ShouldAddNewProject()
        {
            // arrange
            int projectId = 2;
            ProjectBm bm = new ProjectBm()
            {
                Description = "Someshit",
                Id = projectId,
                Plan = "Some big shit",
                ReleaseDate = DateTime.Now,
                Title = "Peeras"
            };

            // act
            this.service.AddAsync(bm, UserA);

            // assert
            Assert.IsNotNull(this.service.Context.Projects.Find(projectId));
            Assert.AreEqual(2, this.service.Context.Projects.Count());
        }

        [TestMethod]
        public void Edit_ShouldEditProject()
        {
            // arrange
            Project beforeEdit = this.service.Context.Projects.First();

            string Description = "Edited Desc";
            int Id = beforeEdit.Id;
            string Plan = "Edited Plan";
            DateTime Date = DateTime.Now;
            string Title = "EditedTitle";

            ProjectBm bm = new ProjectBm()
            {
                Description = Description,
                Id = Id,
                Plan = Plan,
                ReleaseDate = Date,
                Title = Title,
            };

            // act
            this.service.Edit(bm);
            Project afterEdit = this.context.Projects.Find(Id);

            // assert
            Assert.AreNotEqual(beforeEdit.Description, afterEdit.Description);
            Assert.AreNotEqual(beforeEdit.Plan, afterEdit.Plan);
            Assert.AreNotEqual(beforeEdit.Title, afterEdit.Title);
            Assert.AreNotEqual(beforeEdit.ReleaseDate, afterEdit.ReleaseDate);

            Assert.AreEqual(Id, afterEdit.Id);
            Assert.AreEqual(Description, afterEdit.Description);
            Assert.AreEqual(Plan, afterEdit.Plan);
            Assert.AreEqual(Title, afterEdit.Title);
            Assert.AreEqual(Date, afterEdit.ReleaseDate);
            
        }

        [TestMethod]
        public async Task<int> FindAsync_ShouldFindProjectById()
        {
            // arrange
            int id = 1;
            Project expected = this.service.Context.Projects.First();

            // act
            Project result = await this.service.FindAsync(id);

            // assert
            Assert.AreEqual(expected.Id, result.Id);

            return 1;
        }

    }
}
