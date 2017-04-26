using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using Microsoft.AspNet.Identity;

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
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            return View(await this.service.GetUserProjectsToList(filter, page, this.User.Identity.GetUserId()));
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

            ProjectDetailedUserVm vm = this.service.GetDetailedVm(p);

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
                this.service.AddProject(bm, this.User.Identity.GetUserId());
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
            if(project.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
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
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,ReleaseDate,Plan,Description,Title")] ProjectBm bm)
        {
            
            if (!ModelState.IsValid)
            {
                return View(bm);
            }

            Project p = await this.service.FindProjectAsync(bm.Id);

            if(p.Creator.Id != this.User.Identity.GetUserId() && this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            this.service.UpdateProject(p, bm);
            return RedirectToAction("Details", new { id = bm.Id });
            
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
            if(p.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpUnauthorizedResult();
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
                this.service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
