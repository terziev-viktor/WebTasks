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
using WebTasks.Areas.Admin.Controllers;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using WebTasks.Services.Interfaces;

namespace WebTasks.Tests.Controllers.Admin
{
    [TestClass]
    public class DailyTasksContrillerTests : TestClass
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
            var model = new DailyTaskDetailedAdminVm()
            {
                Creator = UserA.UserName,
                Deadline = DateTime.Now,
                Description = randomStr,
                Id = id,
                CommentsCount = 3,
                Note = randomStr,
                Title = randomStr
            };

            serviceMock.Setup(x => x.GetDetailedDailyTaskAdminVmAsync(It.IsAny<int>()))
                .Returns(model);

            // act
            var result = controller.Details(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(((result as ViewResult).Model), typeof(DailyTaskDetailedAdminVm));
            Assert.AreEqual(model.Creator, ((result as ViewResult).Model as DailyTaskDetailedAdminVm).Creator);
            Assert.AreEqual(model.Description, ((result as ViewResult).Model as DailyTaskDetailedAdminVm).Description);
            Assert.AreEqual(model.Id, ((result as ViewResult).Model as DailyTaskDetailedAdminVm).Id);
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

            // act
            var result = this.controller.Edit(bm);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(ActionResult));
            Assert.AreEqual("Details", (result.Result as RedirectToRouteResult).RouteValues.Values.ElementAt(1));
            Assert.AreEqual(1, (result.Result as RedirectToRouteResult).RouteValues.Values.ElementAt(0));
        }

    }
}
