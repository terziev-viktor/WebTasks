using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using System.Collections.Generic;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsService service = new ProjectsService();

        // GET: Admin/Projects
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            IEnumerable<Project> p = await this.service.GetProjectsToListAsync(filter, page);
            IEnumerable<ProjectAdminVm> vm = this.service.MapToProjectsAdminVm(p);

            return View(vm);
        }

        // GET: Admin/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailedAdminVm vm = await this.service.GetProjectDetailedAdminVm(id);
            if (vm == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // GET: Admin/Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReleaseDate,Plan,Title,Description")] ProjectBm bm)
        {
            if (ModelState.IsValid)
            {
                await this.service.AddProjectAsync(bm, this.User.Identity.Name);
                
                return RedirectToAction("Index");
            }

            return View(bm);
        }

        // GET: Admin/Projects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await this.service.FindProjectAsync(id);
            ProjectAdminVm vm = this.service.MapToProjectAdminVm(project);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReleaseDate,Plan,Title,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                this.service.UpdateProject(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await this.service.FindProjectAsync(id);
            ProjectAdminVm vm = this.service.MapToProjectAdminVm(project);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // POST: Admin/Projects/Delete/5
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
                this.service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
