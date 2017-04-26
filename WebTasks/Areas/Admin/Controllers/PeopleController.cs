using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Services;
using WebTasks.Areas.Admin.Models.ViewModels;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PeopleController : Controller
    {
        private readonly PeopleService service = new PeopleService();
        private readonly DailyTasksService dailyTaskService = new DailyTasksService();
        private readonly ProjectsService projectsService = new ProjectsService();

        // GET: Admin/Users
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            return View(await this.service.GetAllUsersAync(filter, page));
        }

        // GET: Admin/Users/Details/5
        public async Task<ActionResult> Details(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PersonDetailedAdminVm vm = await this.service.GetPersonDetailedAdminVm(userId);

            if (vm == null)
            {
                return HttpNotFound();
            }
            vm.DailyTasks = await this.dailyTaskService.GetUserDailyTasks(vm.Id);
            vm.Projects = await this.projectsService.GetUserProjects(vm.Id);
            return View(vm);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
