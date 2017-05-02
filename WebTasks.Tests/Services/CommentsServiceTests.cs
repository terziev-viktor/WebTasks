using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Services;

namespace WebTasks.Tests.Services
{
    //IEnumerable<Comment> GetAllComments();



    [TestClass]
    public class CommentsServiceTests : TestClass
    {
        protected CommentsService service;

        [TestInitialize]
        public void Init()
        {
            InitContext();
            this.service = new CommentsService(context);

        }

        [TestMethod]
        public void GetAllComments_ShouldReturnAllComments()
        {
            // act
            List<Comment> result = this.service.GetAllComments().ToList();

            // assert
            Assert.AreEqual(this.context.Comments.Count(), result.Count());
        }

        [TestMethod]
        public void GetVm_ShouldMapToViewModelByGivenCommentObject()
        {
            // arrange
            int id = 1;
            Comment expected = this.context.Comments.Find(id);

            // act
            CommentVm result = this.service.GetVm(expected);

            // assert
            Assert.AreEqual(expected.Content, result.Content);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.PublishDate, result.PublishDate);

        }


        [TestMethod]
        public void GetVm_ShouldMapToViewModelByGivenId()
        {
            // arrange
            int id = 1;
            Comment expected = this.context.Comments.Find(id);

            // act
            CommentVm result = this.service.GetVm(id);

            // assert
            Assert.AreEqual(expected.Content, result.Content);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.PublishDate, result.PublishDate);

        }

        [TestMethod]
        public void Add_ShouldAddComment()
        {
            // Arrange

            string content = "Add_ShouldAddComment";

            // Act
            this.service.Add(content, 1, 0, UserA);

            // Assert
            Assert.AreEqual(1, this.context.DailyTasks.Find(1).Comments.Count);
        }

        [TestMethod]
        public void Find_ShouldFindComment()
        {
            // arrange
            int CommentId = 1;

            // act
            Comment c = service.Find(CommentId);

            // assert
            Assert.AreEqual(c.Id, CommentId);
        }

        [TestMethod]
        public void Exists_ShouldReturnTrue()
        {
            //arrange
            int commentId = 1;

            // act
            bool result = this.service.Exists(commentId);

            // assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GetAllComments_ShouldReturnListOfAllComments()
        {
            // arrange
            List<Comment> list;

            // act
            list = this.service.GetAllComments().ToList();
            
            // assert
            Assert.IsTrue(list.Count > 0);
        }
       
        [TestMethod]
        public async void DeleteAsync_ShouldDeleteComment()
        {
            // arrange
            int id = 1;

            // act
            int x = await this.service.DeleteAsync(id);

            // assert
            Assert.IsNull(this.context.Comments.Find(id));
        }
    }
}
