using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTasks.Areas.User.Controllers;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Services.Interfaces;

namespace WebTasks.Tests.Controllers.User
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
            string uid = "asdfasfsa";
            var project = new Project()
            {
                Description = "desc",
                Id = id,
                Plan = "some plan",
                Title = "asdfasf",
                Creator = UserA,
                ReleaseDate = DateTime.Now
            };
            var vm = new ProjectDetailedUserVm();
            var identityMock = new Mock<IIdentity>();
            identityMock.Setup(x => x.GetUserId()).Returns(uid);
            userMock.SetupGet(x => x.Identity).Returns(identityMock.Object);
            serviceMock.Setup(x => x.FindAsync(id)).Returns(new Task<Project>(() => project));
            serviceMock.Setup(x => x.GetDetailedVm(It.IsAny<Project>(), It.IsAny<string>())).Returns(vm);

            // act
            var result = controller.Details(id);

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

       // [TestMethod]
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

            serviceMock.Setup(x => x.FindAsync(id)).Returns(new Task<Project>(() => project));
            userMock.Setup(x => x.Identity.GetUserId()).Returns(UserA.Id);
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);
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
