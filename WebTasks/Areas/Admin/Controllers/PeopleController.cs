using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using Microsoft.AspNet.Identity;
using WebTasks.Helpers;
using System.Linq;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PeopleController : Controller
    {
        private readonly IPeopleService service;
        private readonly IDailyTasksService dailyTaskService;
        private readonly IProjectsService projectsService;
        private ApplicationUserManager _userManager;

        public PeopleController(IPeopleService ps, IDailyTasksService dts, IProjectsService prs)
        {
            this.service = ps;
            this.dailyTaskService = dts;
            this.projectsService = prs;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.service.Context as ApplicationDbContext));
        }

        // GET: Admin/Users
        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = true;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            string encodedFilter = HtmlSerializer.ToHtmlString(filter);

            return View(await this.service.GetAllUsersAsync(encodedFilter, page));
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = true;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            string encodedFilter = HtmlSerializer.ToHtmlString(filter);
            return PartialView("PeoplePartial", await this.service.GetAllUsersAsync(encodedFilter));
        }

        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            var vm = await this.service.GetAllUsersAsync("", page);
            return PartialView("DailyTasksPartial", vm);
        }
        [HttpGet]
        [Route("Details/{username}")]
        public async Task<ActionResult> Details(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PersonDetailedAdminVm vm = await this.service.GetPersonDetailedAdminVm(username);
            vm.IsAdmin = this._userManager.IsInRole(vm.Id, "Admin");

            if (vm == null)
            {
                return HttpNotFound();
            }
            vm.DailyTasks = await this.dailyTaskService.GetUserDailyTasks(this._userManager.FindByName(username));
            vm.Projects = await this.projectsService.GetUserProjects(vm.Id);
            return View(vm);
        }

        [HttpPost]
        public ActionResult MakeAdmin(string username)
        {
            var user = this._userManager.FindByName(username);
            this._userManager.AddToRole(user.Id, "Admin");
            return new HttpStatusCodeResult(200);
        }
    }
}
