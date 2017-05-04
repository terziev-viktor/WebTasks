using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Services;
using System.Collections.Generic;
using WebTasks.Areas.Admin.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using WebTasks.Filters;
using System.Linq;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        public IProjectsService Service;
        private ApplicationUserManager _userManager;

        public ProjectsController()
        {   }

        public ProjectsController(IProjectsService s)
        {
            this.Service = s;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.Service.Context as ApplicationDbContext));
        }

        // GET: Admin/Projects
        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;

            IEnumerable<Project> p = await this.Service.GetProjectsToListAsync(filter, page);
            IEnumerable<ProjectAdminVm> vm = this.Service.MapToProjectsAdminVm(p);

            return View(vm);
        }

        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;

            IEnumerable<Project> p = await this.Service.GetProjectsToListAsync(filter, page);
            IEnumerable<ProjectAdminVm> vm = this.Service.MapToProjectsAdminVm(p);

            return PartialView("ProjectsPartial", vm);
        }

        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.Projects.Count();
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<Project> p = await this.Service.GetProjectsToListAsync("", page);
            IEnumerable<ProjectAdminVm> vm = this.Service.MapToProjectsAdminVm(p);
            
            return PartialView("ProjectsPartial", vm);
        }

        // GET: Admin/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectDetailedAdminVm vm = await this.Service.GetProjectDetailedAdminVm(id);

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
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "ReleaseDate,Plan,Title,Description")] ProjectBm bm)
        {
            if (ModelState.IsValid)
            {
                // returns id of inserted project
                int id = await this.Service.CreateAsync(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
                
                return RedirectToAction("Details", new { id = id });
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
            Project project = await this.Service.FindAsync(id);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,ReleaseDate,Plan,Description,Title,EditedReleaseDate")] ProjectBm bm)
        {
            if (!ModelState.IsValid)
            {
                return View(bm);
            }

            await this.Service.Edit(bm);
            return RedirectToAction("Details", new { id = bm.Id });

        }

        // GET: Admin/Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await this.Service.FindAsync(id);
            ProjectAdminVm vm = this.Service.MapToProjectAdminVm(project);

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
            Project project = await this.Service.FindAsync(id);
            await this.Service.RemoveAsync(project);
            return RedirectToAction("Index");
        }
    }
}
