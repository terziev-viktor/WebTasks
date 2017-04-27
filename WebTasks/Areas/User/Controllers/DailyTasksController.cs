using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Models.BindingModels;
using WebTasks.Services;
using WebTasks.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class DailyTasksController : Controller
    {
        private readonly DailyTasksService service = new DailyTasksService();

        // GET: User/DailyTasks
        public async Task<ActionResult> Index(string filter = "", int page=1)
        {
            ViewBag.Header = "Your daily tasks";
            IEnumerable<DailyTaskVm> tasksVm = await this.service.GetUserDailyTasksVm(filter, page, this.User.Identity.GetUserId());
            return View(tasksVm);
        }

        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            IEnumerable<DailyTaskVm> tasksVm = await this.service.GetUserDailyTasksVm(filter, page, this.User.Identity.GetUserId());

            return PartialView("DailyTasksPartial", tasksVm);
        }

        // User/DailyTasks/Page/5
        [HttpGet]
        [Route("Page/{id}")]
        public async Task<ActionResult> Page(int id)
        {
            IEnumerable<DailyTaskVm> vm = await this.service.GetUserDailyTasksVm("", id, this.User.Identity.GetUserId());

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

            var vm = this.service.GetDetailedDailyTaskVm(id);
            
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
                await this.service.CreateNewTask(bm, this.User.Identity.GetUserId());
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

            DailyTask dailyTask = await this.service.FindDailyTaskById(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if(!this.service.IsOwner(bm.Id, this.User.Identity.GetUserId()) && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (ModelState.IsValid)
            {
                await this.service.Edit(bm);
                return RedirectToAction("Details", new { id = bm.Id});
            }
            return View(bm);
        }

        // GET: User/DailyTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await this.service.FindDailyTaskById(id);
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
            DailyTask dailyTask = await this.service.FindDailyTaskById(id);
            this.service.Remove(dailyTask);
            await this.service.SaveChangesAsync();
            return RedirectToAction("Index");
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
