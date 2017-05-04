using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebTasks.Areas.User.Controllers;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Services.Interfaces;

namespace WebTasks.Tests.Controllers.User
{
    [TestClass]
    public class DailyTasksControllerTests : TestClass
    {
        private DailyTasksController controller;
        private Mock<IDailyTasksService> serviceMock;
        private Mock<IPrincipal> userMock;
        private Mock<HttpContextBase> contextMock;
        private Mock<ControllerContext> controllerContextMock;

        [TestInitialize]
        public void Init()
        {
            InitContext();
            controller = new DailyTasksController();
            serviceMock = new Mock<IDailyTasksService>();
            controller.Service = serviceMock.Object;
            userMock = new Mock<IPrincipal>();
            userMock.SetupGet(x => x.Identity.IsAuthenticated).Returns(true);
            

            contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controller.ControllerContext = controllerContextMock.Object;
        }

        [TestMethod]
        public void GetDetails_ShouldReturnViewWithModel()
        {
            // arrange
            string randomStr = "Some <script> alert() </script>";
            int id = 2121;
            var model = new DailyTaskDetailedUserVm()
            {
                Deadline = DateTime.Now,
                Description = randomStr,
                Id = id,
                CommentsCount = 3,
                Note = randomStr,
                Title = randomStr
            };

            serviceMock.Setup(x => x.GetDetailedDailyTaskVm(It.IsAny<int>()))
                .Returns(model);

            // act
            var result = controller.Details(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(((result as ViewResult).Model), typeof(DailyTaskDetailedUserVm));
            Assert.AreEqual(model.Description, ((result as ViewResult).Model as DailyTaskDetailedUserVm).Description);
            Assert.AreEqual(model.Id, ((result as ViewResult).Model as DailyTaskDetailedUserVm).Id);
        }

        [TestMethod]
        public void GetCreate_ShouldReturnView()
        {

            // act
            var result = controller.Create();

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostEdit_ShouldReturnRedirectAction()
        {
            // arrange
            var bm = new DailyTaskBm()
            {
                Deadline = DateTime.Now,
                Id = 1,
                Description = "some string",
                Note = "some note",
                Title = "some title"
            };
            userMock.Setup(x => x.IsInRole("Admin")).Returns(false);
            userMock.Setup(x => x.IsInRole("User")).Returns(true);
            serviceMock.Setup(x => x.IsOwner(It.IsAny<int>(), null)).Returns(true);

            // act
            var result = this.controller.Edit(bm);

            // assert
            Assert.IsNotNull(result);
        }

    }
}
