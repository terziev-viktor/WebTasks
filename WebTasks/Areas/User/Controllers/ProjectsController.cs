using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Areas.User.Models.ViewModels;
using WebTasks.Models.BindingModels;
using WebTasks.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using WebTasks.Services.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using WebTasks.Models;
using WebTasks.Helpers;
using System.Linq;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        public IProjectsService Service;
        private ApplicationUserManager _userManager;

        // An empty constructor for unit test purposes
        public ProjectsController()
        {   }

        public ProjectsController(IProjectsService s)
        {
            this.Service = s;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.Service.Context as ApplicationDbContext));

        }

        // GET: User/Projects
        [HttpGet]

        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;

            return View(await this.Service.GetUserProjectsToList(filter, page, this._userManager.FindById(this.User.Identity.GetUserId())));
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.Service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;
            
            return PartialView("ProjectsPartial", await this.Service.GetUserProjectsToList(filter, page, this._userManager.FindById(this.User.Identity.GetUserId())));
        }

        // User/DailyTasks/Page/5
        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<ProjectVm> vm = await this.Service.GetUserProjectsToList("", page, this._userManager.FindById(this.User.Identity.GetUserId()));
            ViewBag.IsLastPage = page * 10 >= this.Service.GetUserProjectsCount(this.User.Identity.GetUserId());

            return PartialView("ProjectsPartial", vm);
        }

        // GET: User/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.Service.FindAsync(id);

            if (p == null)
            {
                return HttpNotFound();
            }

            ProjectDetailedUserVm vm = this.Service.GetDetailedVm(p);

            return View(vm);
        }

        // GET: User/Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Projects/Create
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "ReleaseDate,Title")] ProjectBm bm)
        {
            if (ModelState.IsValid)
            {
                // id of inserted object
                int id = await this.Service.CreateAsync(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
                return RedirectToAction("Details", new { id = id });
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
            Project project = await Service.FindAsync(id);

            if (project == null)
            {
                return HttpNotFound();
            }
            if (project.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            
            return View(project);
        }

        // POST: User/Projects/Edit/5
        // Disable validateinput so the service can serialize the input. This way the user can post
        // input that is HTML
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,ReleaseDate,Plan,Description,Title,EditedReleaseDate")] ProjectBm bm)
        {
            
            if (!ModelState.IsValid)
            {
                return View(bm);
            }

            Project p = await this.Service.FindAsync(bm.Id);

            if(p.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            await this.Service.Edit(bm);
            return RedirectToAction("Details", new { id = bm.Id });
            
        }

        // GET: User/Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.Service.FindAsync(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            if(p.Creator.Id != this.User.Identity.GetUserId() && !this.User.IsInRole("Admin"))
            {
                return new HttpUnauthorizedResult();
            }
            ProjectVm vm = this.Service.GetProjectVm(p);

            return View(vm);
        }

        // POST: User/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await this.Service.FindAsync(id);
            if(project == null)
            {
                return HttpNotFound();
            }
            await this.Service.RemoveAsync(project);
            return RedirectToAction("Index");
        }
    }
}
