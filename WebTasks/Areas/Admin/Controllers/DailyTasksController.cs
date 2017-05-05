using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using Microsoft.AspNet.Identity;
using WebTasks.Helpers;
using System.Linq;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyTasksController : Controller
    {
        public IDailyTasksService Service; // Set as public for unit test purposes
        private ApplicationUserManager _userManager;

        // An empty constructor for unit test purposes
        public DailyTasksController()
        {

        }

        public DailyTasksController(IDailyTasksService s)
        {
            this.Service = s;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.Service.Context as ApplicationDbContext));
        }

        // GET: Admin/DailyTasks
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            // page number is passed to the method and updated in the view so that every
            // time new number will be passed
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = true;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.DailyTasks.Count();

            IEnumerable<DailyTaskAdminVm> data = await this.Service.GetAllDailyTaskVm(filter, page);
            return View(data);
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            // page number is passed to the method and updated in the view so that every
            // time new number will be passed
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = true;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.DailyTasks.Count();

            string encodedFilter = HtmlSerializer.ToEncodedString(filter);
            IEnumerable<DailyTaskAdminVm> data = await this.Service.GetAllDailyTaskVm(encodedFilter, page);
            return PartialView("DailyTasksPartial", data);
        }

        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            // page number is passed to the method and updated in the view so that every
            // time new number will be passed
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.DailyTasks.Count();
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<DailyTaskAdminVm> vm = await this.Service.GetAllDailyTaskVm("", page);
            return PartialView("DailyTasksPartial", vm);
        }

        // GET: Admin/DailyTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DailyTaskDetailedAdminVm vm = this.Service.GetDetailedDailyTaskAdminVmAsync(id);
            
            if (vm == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        // GET: Admin/DailyTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DailyTasks/Create
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                // returns id of inserted object
                int id = await this.Service.Create(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
                return RedirectToAction("Details", new { id = id });
            }

            return View(bm);
        }

        // GET: Admin/DailyTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: Admin/DailyTasks/Edit/5
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description,EditedDeadline")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                await this.Service.Edit(bm);
                return RedirectToAction("Details", new { id = bm.Id });
            }
            return View(bm);
        }

        // GET: Admin/DailyTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: Admin/DailyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DailyTask dailyTask = await this.Service.FindByIdAsync(id);
            if(dailyTask == null)
            {
                return HttpNotFound();
            }
            await this.Service.Remove(dailyTask);
            return RedirectToAction("Index");
        }
    }
}
