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
using WebTasks.Filters;
using WebTasks.Helpers;
using System.Linq;

namespace WebTasks.Areas.User.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private IProjectsService service;
        private ApplicationUserManager _userManager;

        public ProjectsController(IProjectsService s)
        {
            this.service = s;
            this._userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.service.Context as ApplicationDbContext));

        }

        // GET: User/Projects
        [HttpGet]

        public async Task<ActionResult> Index(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;

            return View(await this.service.GetUserProjectsToList(filter, page, this._userManager.FindById(this.User.Identity.GetUserId())));
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ActionResult> Filter(string filter = "", int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsLastPage = page * 10 >= this.service.Context.Projects.Count();
            ViewBag.IsFirstPage = true;

            string encoded = HtmlSerializer.ToHtmlString(filter);

            return PartialView("ProjectsPartial", await this.service.GetUserProjectsToList(encoded, page, this._userManager.FindById(this.User.Identity.GetUserId())));
        }

        // User/DailyTasks/Page/5
        [HttpGet]
        [Route("Page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            ViewBag.CurrentPage = page;
            ViewBag.IsFirstPage = page == 1;

            IEnumerable<ProjectVm> vm = await this.service.GetUserProjectsToList("", page, this._userManager.FindById(this.User.Identity.GetUserId()));
            ViewBag.IsLastPage = page * 10 >= this.service.GetUserProjectsCount(this.User.Identity.GetUserId());

            return PartialView("ProjectsPartial", vm);
        }

        // GET: User/Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.service.FindAsync(id);

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
                await this.service.AddAsync(bm, this._userManager.FindById(this.User.Identity.GetUserId()));
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
            Project project = await service.FindAsync(id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,ReleaseDate,Plan,Description,Title,EditedReleaseDate")] ProjectBm bm)
        {
            
            if (!ModelState.IsValid)
            {
                return View(bm);
            }

            Project p = await this.service.FindAsync(bm.Id);

            if(p.Creator.Id != this.User.Identity.GetUserId() && this.User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            await this.service.Edit(bm);
            return RedirectToAction("Details", new { id = bm.Id });
            
        }

        // GET: User/Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project p = await this.service.FindAsync(id);
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
            Project project = await this.service.FindAsync(id);
            if(project == null)
            {
                return HttpNotFound();
            }
            await this.service.RemoveAsync(project);
            return RedirectToAction("Index");
        }
    }
}
