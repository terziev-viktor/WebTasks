using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Services;
using System.Collections.Generic;
using WebTasks.Areas.User.Models.ViewModels;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private readonly PeopleService service = new PeopleService();
        private readonly DailyTasksService dailyTaskService = new DailyTasksService();
        private readonly ProjectsService projectsService = new ProjectsService();

        // GET: User/People
        public async Task<ActionResult> Index(string filter, int page)
        {
            IEnumerable<PersonVm> vm = await this.service.GetAllUsersByUserName(filter, page);
            return View(vm);
        }

        // GET: User/People/Details/5
        public async Task<ActionResult> Details(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            PersonDetailedVm vm = await this.service.GetPersonDetailedUserVm(userId);

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
