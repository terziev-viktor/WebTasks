using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Models.BindingModels;
using WebTasks.Services;
using WebTasks.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using WebTasks.Filters;
using WebTasks.Helpers;
using WebTasks.Areas.User.Models.ViewModels;
using System.Linq;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class DailyTasksController : Controller
    {
        private readonly IDailyTasksService service;
        private ApplicationUserManager _userManager;

        public DailyTasksController(IDailyTasksService sr)
        {
            this.service = sr;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.service.Context as ApplicationDbContext));

        }

        // GET: User/DailyTasks
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.service.Context.DailyTasks.Count();
            ViewBag.IsFirstPage = page == 1;

            ViewBag.Header = "Your daily tasks";
            IEnumerable<DailyTaskVm> tasksVm = await this.service.GetUserDailyTasksVm(filter, page, this._userManager.FindById(this.User.Identity.GetUserId()));
            return View(tasksVm);
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page * 10 >= this.service.Context.DailyTasks.Count();

            string encoded = HtmlSerializer.ToHtmlString(filter);
            IEnumerable<DailyTaskVm> tasksVm = await this.service.GetUserDailyTasksVm(encoded, page, this._userManager.FindById(this.User.Identity.GetUserId()));

            return PartialView("DailyTasksPartial", tasksVm);
        }

        // User/DailyTasks/Page/5
        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<DailyTaskVm> vm = await this.service.GetUserDailyTasksVm("", page, this._userManager.FindById(this.User.Identity.GetUserId()));
            ViewBag.IsLastPage = page * 10 >= this.service.GetUserDailyTasksCount(this.User.Identity.GetUserId());

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

            DailyTaskDetailedUserVm vm = this.service.GetDetailedDailyTaskVm(id);
            
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                await this.service.CreateFromBm(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
                return RedirectToAction("Index");
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

            DailyTask dailyTask = await this.service.FindByIdAsync(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description,EditedDeadline")] DailyTaskBm bm)
        {
            if(!this.service.IsOwner(bm.Id, this._userManager.FindById(this.User.Identity.GetUserId())) && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (ModelState.IsValid)
            {
                await this.service.Edit(bm);
                return RedirectToAction("Details", new { id = bm.Id});
            }
            return View(this.service.Map(bm));
        }

        // GET: User/DailyTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await this.service.FindByIdAsync(id);
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
            DailyTask dailyTask = await this.service.FindByIdAsync(id);
            await this.service.Remove(dailyTask);
            return RedirectToAction("Index");
        }
    }
}
