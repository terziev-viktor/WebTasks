using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Models.BindingModels;
using AutoMapper;
using WebTasks.Services;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class DailyTasksController : Controller
    {
        private DailyTasksService service;
        public DailyTasksController()
        {
            service = new DailyTasksService();
        }

        // GET: User/DailyTasks
        public ActionResult Index()
        {
            ViewBag.Header = "Your daily tasks";
            var tasksVm = this.service.GetUserDailyTasksVm(this.User.Identity.Name);
            return View(tasksVm);
        }

        // GET: User/DailyTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vm = this.service.GetDetailedDailyTaskVm(id);

            if(vm == null)
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

        // GET: User/DailyTasks/Edit/5
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

        // POST: User/DailyTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description")] DailyTaskBm bm)
        {
            if (ModelState.IsValid)
            {
                DailyTask dt = Mapper.Instance.Map<DailyTaskBm, DailyTask>(bm);
                this.service.SetEntryState(dt, EntityState.Modified);
                await this.service.SaveChangesAsync();
                return RedirectToAction("Index");
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
                this.service.DisposeContext();
            }
            base.Dispose(disposing);
        }
    }
}
