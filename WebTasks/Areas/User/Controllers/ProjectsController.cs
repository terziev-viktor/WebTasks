using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ProjectsService service;

        public ProjectsController()
        {
            this.service = new ProjectsService();
        }

        // GET: User/Projects
        public ActionResult Index()
        {
            return View(this.service.GetProjectsToList());
        }

        // GET: User/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.service.FindProjectAsync(id);

            if (p == null)
            {
                return HttpNotFound();
            }

            ProjectDetailedVm vm = this.service.GetDetailedVm(p);

            return View(vm);
        }

        // GET: User/Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReleaseDate,Title")] ProjectBm bm)
        {
            if (ModelState.IsValid)
            {
                this.service.AddProject(bm);
                
                await this.service.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bm);
        }

        // GET: User/Projects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await service.FindProjectAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectVm vm = this.service.GetProjectVm(project);

            return View(vm);
        }

        // POST: User/Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ReleaseDate,Plan,Title,Description")] ProjectBm bm)
        {
            if (ModelState.IsValid)
            {
                Project p = await this.service.FindProjectAsync(bm.Id);
                this.service.UpdateProject(p, bm.ReleaseDate, bm.Plan, bm.Title, bm.Description);
                this.service.SetEntryState(p, EntityState.Modified);
                await this.service.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bm);
        }

        // GET: User/Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.service.FindProjectAsync(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            ProjectVm vm = this.service.GetProjectVm(p);

            return View(vm);
        }

        // POST: User/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await this.service.FindProjectAsync(id);
            this.service.RemoveProject(project);
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
