using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebTasks.Areas.Admin.Controllers;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Services.Interfaces;

namespace WebTasks.Tests.Controllers.Admin
{
    [TestClass]
    public class ProjectsControllerTests : TestClass
    {
        private ProjectsController controller;
        private Mock<IProjectsService> serviceMock;
        private Mock<IPrincipal> userMock;
        private Mock<HttpContextBase> contextMock;
        private Mock<ControllerContext> controllerContextMock;


        [TestInitialize]
        public void Init()
        {
            InitContext();
            controller = new ProjectsController();
            serviceMock = new Mock<IProjectsService>();
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
            int id = 1;
            var vm = new ProjectDetailedAdminVm();

            serviceMock.Setup(x => x.GetProjectDetailedAdminVm(id)).Returns(new System.Threading.Tasks.Task<ProjectDetailedAdminVm>(() => vm ));

            // act
            var result = controller.Details((int?)id);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(vm, (result.Result as ViewResult).Model);
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
            int id = 1;
            var bm = new ProjectBm()
            {
                ReleaseDate = DateTime.Now,
                Id = id,
                Description = "some string",
                Plan = "some thing",
                Title = "some other title"
            };
            var project = new Project()
            {
                Description = "desc",
                Id = id,
                Plan = "some plan",
                Title = "asdfasf",
                Creator = UserA,
                ReleaseDate = DateTime.Now
            };
            
            serviceMock.Setup(x => x.Edit(It.IsAny<ProjectBm>())).Returns(new System.Threading.Tasks.Task(() => { }));

            // act
            var result = this.controller.Edit(bm);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", (result.Result as RedirectToRouteResult).RouteValues.Values.ElementAt(1));
            Assert.AreEqual(1, (result.Result as RedirectToRouteResult).RouteValues.Values.ElementAt(0));
        }
    }
}
