using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyTasksController : Controller
    {
        private readonly DailyTasksService service = new DailyTasksService();

        // GET: Admin/DailyTasks
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            IEnumerable<DailyTaskAdminVm> data = await this.service.GetAllDailyTaskVm(filter, page);
            return View(data);
        }

        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            IEnumerable<DailyTaskAdminVm> data = await this.service.GetAllDailyTaskVm(filter, page);
            return PartialView("DailyTasksPartial", data);
        }

        [HttpGet]
        [Route("Page/{id}")]
        public async Task<ActionResult> Page(int id)
        {
            IEnumerable<DailyTaskAdminVm> vm = await this.service.GetAllDailyTaskVm("", id);
            return PartialView("DailyTasksPartial", vm);
        }

        // GET: Admin/DailyTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTaskDetailedAdminVm vm = this.service.GetDetailedDailyTaskAdminVmAsync(id);
            
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                await this.service.CreateNewTask(bm, this.User.Identity.Name);
                return RedirectToAction("Index");
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
            DailyTask dailyTask = await this.service.FindDailyTaskById(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: Admin/DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                await this.service.Edit(bm);
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
            DailyTask dailyTask = await this.service.FindDailyTaskById(id);
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
