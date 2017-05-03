using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTasks.Controllers;
using WebTasks.Models;
using WebTasks.Models.EntityModels;
using WebTasks.Models.Interfaces;
using WebTasks.Models.ViewModels;
using WebTasks.Services;
using WebTasks.Services.Interfaces;

namespace WebTasks.Tests.Controllers
{
    [TestClass]
    public class CommentsControllerTest : TestClass
    {

        [TestInitialize]
        public void Init()
        {
            this.InitContext();
        }

        [TestMethod]
        public void Index_ShouldReturnViewWithModel()
        {
            var controller = new CommentsController();
            var serviceMock = new Mock<ICommentsService>();
            serviceMock.Setup(x => x.GetAllComments()).Returns(new List<Comment>()
            {
                new Comment()
                {
                    Content = "Some content",
                    PublishDate = DateTime.Now,
                    Author = UserA
                }
            });
            controller.Service = serviceMock.Object;

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);
            userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controller.ControllerContext = controllerContextMock.Object;

            // act
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType((result as ViewResult).Model, typeof(IEnumerable<Comment>));
            Assert.AreEqual(((result as ViewResult).Model as IEnumerable<Comment>).Count(), 1);
        }

        [TestMethod]
        public void Edit_ShouldReturnViewWithModel()
        {
            int id = 1;
            string modelContent = "Some content";

            var controller = new CommentsController();
            var serviceMock = new Mock<ICommentsService>();
            serviceMock.Setup(x => x.GetComment(id)).Returns(
                new Comment()
                {
                    Content = modelContent,
                    PublishDate = DateTime.Now,
                    Author = UserA
                });
            controller.Service = serviceMock.Object;

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);
            userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controller.ControllerContext = controllerContextMock.Object;

            // act
            var result = controller.Edit(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType((result as ViewResult).Model, typeof(Comment));
            Assert.AreEqual(((result as ViewResult).Model as Comment).Content, modelContent);
        }

        [TestMethod]
        public void Create_ShouldReturnPartialView()
        {
            // arrange
            string content = "Some content";
            int forTask = 1;
            int taskType = 0;

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);
            userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            var controller = new CommentsController();
            var userStoreMock = new Mock<UserStore<ApplicationUser>>(controllerContextMock.Object);
            var serviceMock = new Mock<ICommentsService>();
            var userManagerMock = new Mock<ApplicationUserManager>();
            userManagerMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(UserA);
            controller.UserManager = userManagerMock.Object;

            serviceMock.Setup(x => x.Add(content, forTask, taskType, UserA)).Returns(new Comment()
            {
                Content = content,
                Author = UserA,
                DailyTaskId = forTask,
                PublishDate = DateTime.Now
            });

            serviceMock.Setup(x => x.GetVm(It.IsAny<Comment>())).Returns(new CommentVm()
            {
                Content = content,
                Author = UserA.UserName,
                PublishDate = DateTime.Now
            });

            controller.Service = serviceMock.Object;

            

            controller.ControllerContext = controllerContextMock.Object;

            
            // act
            var result = controller.Create(content, forTask, taskType);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(((result as PartialViewResult).Model as CommentVm).Content, content);
            Assert.AreEqual(((result as PartialViewResult).Model as CommentVm).Author, UserA.UserName);
        }
    }
}
