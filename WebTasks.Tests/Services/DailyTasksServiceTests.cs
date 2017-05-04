using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Services;

namespace WebTasks.Tests.Services
{
    [TestClass]
    public class DailyTasksServiceTests : TestClass
    {
        protected DailyTasksService service;

        [TestInitialize]
        public void Init()
        {
            InitContext();
            service = new DailyTasksService(context);
        }

        [TestMethod]
        public void UserDailyTasksCount_ShouldReturnNumberOfDailyTasksCreatedByUser()
        {
            // arrange
            string id = UserA.Id;
            int expected = this.context.DailyTasks.Count(x => x.Creator.Id == id);

            // act
            int result = this.service.GetUserDailyTasksCount(id);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateAsync_ShouldCreateNewDailyTask()
        {
            // arrange
            int exprectedCommentsCountAfterCreate = 2;

            DateTime deadline = DateTime.Now;
            string desc = "Some desc";
            int id = 1000;
            string note = "some note";
            string title = "Some title";

            DailyTaskBm bm = new DailyTaskBm()
            {
                Deadline = deadline,
                Description = desc,
                Id = id,
                Note = note,
                Title = title
            };

            // act
            this.service.Create(bm, UserA);

            // assert
            Assert.AreEqual(this.context.DailyTasks.Count(), exprectedCommentsCountAfterCreate);
        }

        [TestMethod]
        public void EditAsync_ShouldEditTask()
        {
            // arrange
            int id = 1;
            string description = this.context.DailyTasks.Find(id).Description;
            string title = this.context.DailyTasks.Find(id).Title;
            string note = this.context.DailyTasks.Find(id).Note;
            DateTime deadline = this.context.DailyTasks.Find(id).Deadline;

            string newDescription = "Some edited desc";
            string editedNote = "Some edited shit";
            DateTime editedDeadline = DateTime.Now;
            string editedTitle = "Some edited shit";

            DailyTaskBm bm = new DailyTaskBm()
            {
                Id = 1, // should edit dailytask with id of bm
                Deadline = editedDeadline,
                Title = editedTitle,
                Note = editedNote,
                Description = newDescription
            };

            // act (running sync)
            this.service.Edit(bm);

            // assert
            Assert.AreNotEqual(title, this.context.DailyTasks.Find(id).Title);
            Assert.AreNotEqual(description, this.context.DailyTasks.Find(id).Description);
            Assert.AreNotEqual(note, this.context.DailyTasks.Find(id).Note);
            Assert.AreNotEqual(deadline, this.context.DailyTasks.Find(id).Deadline);

            Assert.AreEqual(editedNote, this.context.DailyTasks.Find(id).Note);
            Assert.AreEqual(editedTitle, this.context.DailyTasks.Find(id).Title);
            Assert.AreEqual(editedNote, this.context.DailyTasks.Find(id).Note);
            Assert.AreEqual(editedDeadline, this.context.DailyTasks.Find(id).Deadline);
        }

        [TestMethod]
        public void IsOwner_ShouldReturnTrue()
        {
            // arrange
            int taskId = 1;

            // act
            bool result = this.service.IsOwner(taskId, UserA);

            // assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FindById_ShouldFindDailyTaskById()
        {
            // arrange
            int id = 1;
            DailyTask expected = this.service.Context.DailyTasks.First();

            //act
            DailyTask result = this.service.FindById(id);

            // assert
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.Deadline, result.Deadline);
            Assert.AreEqual(expected.Note, result.Note);

        }

        [TestMethod]
        public void GetAllDailyTasksVm_ShouldReturnAllTasksAsViewModels()
        {
            // arrange
            int expectedCount = this.service.Context.DailyTasks.Count();

            // assert
            var result = this.service.GetAllDailyTaskVm("", 10);
            var r = result.Result.ToList();

            // assert
            Assert.AreEqual(expectedCount, r.Count());
        }

        [TestMethod]
        public void GetDetailedDailyTaskVm_ShouldReturnDailyTaskAsDetailerViewModel()
        {
            int id = 1;
            DailyTask expected = this.service.Context.DailyTasks.First();

            DailyTaskDetailedUserVm result = this.service.GetDetailedDailyTaskVm(id);

            Assert.IsInstanceOfType(result, typeof(DailyTaskDetailedUserVm));
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.Note, result.Note);
        }

        [TestMethod]
        public void Map_ShouldMapBindingModelToDailyTaskObject()
        {
            // arrange
            int Id = 5;
            string Description = "SomeShit";
            string Note = "Some note";
            string Title = "Some titile";
            DateTime Deadline = DateTime.Now;
            DailyTaskBm bm = new DailyTaskBm()
            {
                Id = Id,
                Description = Description,
                Note = Note,
                Title = Title,
                Deadline = Deadline
            };

            // act
            DailyTask result = this.service.Map(bm);

            //assert
            Assert.AreEqual(Id, result.Id);
            Assert.AreEqual(Description, result.Description);
            Assert.AreEqual(Note, result.Note);
            Assert.AreEqual(Title, result.Title);
            Assert.AreEqual(Deadline, result.Deadline);
        }
        
        [TestMethod]
        public void RemoveAsync_ShouldRemoveDailyTask()
        {
            // arrange
            Models.EntityModels.DailyTask dt = this.context.DailyTasks.First();
            int dailyTasksCount = this.context.DailyTasks.Count();

            // act
            this.service.Remove(dt);

            // assert
            Assert.AreEqual(dailyTasksCount - 1, this.context.DailyTasks.Count());
        }
    }
}
