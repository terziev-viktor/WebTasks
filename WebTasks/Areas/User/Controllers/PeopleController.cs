using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Services;
using System.Collections.Generic;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.ViewModels;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using Microsoft.AspNet.Identity;
using WebTasks.Helpers;
using System.Linq;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly IPeopleService service;
        private readonly IDailyTasksService dailyTaskService;
        private readonly Services.Interfaces.IProjectsService projectsService;
        private ApplicationUserManager _userManager;

        public PeopleController(IPeopleService ps, IDailyTasksService dts, Services.Interfaces.IProjectsService prs)
        {
            this.service = ps;
            this.dailyTaskService = dts;
            this.projectsService = prs;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.service.Context as ApplicationDbContext));

        }

        // GET: User/People
        [ValidateInput(false)]
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            string encoded = HtmlSerializer.ToHtmlString(filter);
            IEnumerable<PersonVm> vm = await this.service.GetAllUsersAsync(encoded, page);
            return View(vm);
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            string encoded = HtmlSerializer.ToHtmlString(filter);
            
            IEnumerable<PersonVm> vm = await this.service.GetAllUsersAsync(encoded);
            return PartialView("PeoplePartial", vm);
        }

        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this._userManager.Users.Count();

            var vm = await this.service.GetAllUsersAsync("", page);
            return PartialView("PeoplePartial", vm);
        }

        // GET: User/People/Details/user@u.com
        [Route("Details/{username}")]
        public async Task<ActionResult> Details(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            PersonDetailedVm vm = await this.service.GetPersonDetailedUserVm(username);

            if (vm == null)
            {
                return HttpNotFound();
            }

            vm.DailyTasks = await this.dailyTaskService.GetUserDailyTasks(this._userManager.FindByName(username));
            vm.Projects = await this.projectsService.GetUserProjects(vm.Id);
            vm.TasksCount = vm.DailyTasks.Count() + vm.Projects.Count();

            return View(vm);
        }
    }
}
