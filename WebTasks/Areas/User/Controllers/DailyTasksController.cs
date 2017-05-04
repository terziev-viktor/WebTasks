using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using WebTasks.Helpers;
using WebTasks.Areas.User.Models.ViewModels;
using System.Linq;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class DailyTasksController : Controller
    {
        public IDailyTasksService Service; // set as public for unit test purposes

        private ApplicationUserManager _userManager;

        // an empty constructor for unit test purposes
        public DailyTasksController()
        {

        }

        public DailyTasksController(IDailyTasksService sr)
        {
            this.Service = sr;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.Service.Context as ApplicationDbContext));

        }

        // GET: User/DailyTasks
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            // page number is passed to the method and updated in the view so that every
            // time new number will be passed
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.DailyTasks.Count();
            ViewBag.IsFirstPage = page == 1;

            ViewBag.Header = "Your daily tasks";
            IEnumerable<DailyTaskVm> tasksVm = await this.Service.GetUserDailyTasksVm(filter, page, this._userManager.FindById(this.User.Identity.GetUserId()));
            return View(tasksVm);
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            // page number is passed to the method and updated in the view so that every
            // time new number will be passed
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.DailyTasks.Count();
            
            IEnumerable<DailyTaskVm> tasksVm = await this.Service.GetUserDailyTasksVm(filter, page, this._userManager.FindById(this.User.Identity.GetUserId()));

            return PartialView("DailyTasksPartial", tasksVm);
        }

        // User/DailyTasks/Page/5
        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<DailyTaskVm> vm = await this.Service.GetUserDailyTasksVm("", page, this._userManager.FindById(this.User.Identity.GetUserId()));
            ViewBag.IsLastPage = page * 10 >= this.Service.GetUserDailyTasksCount(this.User.Identity.GetUserId());

            return PartialView("DailyTasksPartial", vm);
        }

        // GET: User/DailyTasks/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DailyTaskDetailedUserVm vm = this.Service.GetDetailedDailyTaskVm(id);
            
            if (vm == null)
            {
                return this.HttpNotFound();
            }

            return View(vm);
        }

        // GET: User/DailyTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/DailyTasks/Create
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                int id = await this.Service.Create(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
                return RedirectToAction("Details", new { id = id });
            }

            return View(bm);
        }

        // GET: User/DailyTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            // Only the creator and admins can edit the task
            if(dailyTask.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (dailyTask == null)
            {
                return HttpNotFound();
            }

            return View(dailyTask);
        }

        // POST: User/DailyTasks/Edit/5
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description,EditedDeadline")] DailyTaskBm bm)
        {
            if(!this.Service.IsOwner(bm.Id, this._userManager.FindById(this.User.Identity.GetUserId())) && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (ModelState.IsValid)
            {
                await this.Service.Edit(bm);
                return RedirectToAction("Details", new { id = bm.Id});
            }
            return View(this.Service.Map(bm));
        }

        // GET: User/DailyTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            if (dailyTask.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: User/DailyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            await this.Service.Remove(dailyTask);
            return RedirectToAction("Index");
        }
    }
}
