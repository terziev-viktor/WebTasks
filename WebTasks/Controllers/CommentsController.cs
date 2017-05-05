using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using WebTasks.Filters;
using WebTasks.Helpers;
using WebTasks.Models;
using WebTasks.Models.BindingModels;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Services;
using WebTasks.Services.Interfaces;

namespace WebTasks.Controllers
{
    public class CommentsController : Controller
    {
        public ICommentsService Service;
        public ApplicationUserManager UserManager;

        public CommentsController()
        {

        }

        public CommentsController(ICommentsService s)
        {
            this.Service = s;
            this.UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(this.Service.Context as ApplicationDbContext));
        }

        // GET: Comments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(this.Service.GetAllComments());
        }

        // GET: Comments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(string Content, int ForTask, int TaskType)
        {
            if (Content == null)
            {
                return new HttpStatusCodeResult(403);
            }
            string contentHtmlEncoded = HtmlSerializer.ToEncodedString(Content);
            if (contentHtmlEncoded.Length == 0)
            {
                return new HttpStatusCodeResult(403);
            }
            Comment added = this.Service.Add(contentHtmlEncoded, ForTask, TaskType, this.UserManager.FindById(this.User.Identity.GetUserId()));
            if (added != null)
            {
                CommentVm vm = this.Service.GetVm(added);
                return PartialView("CommentPosted", vm);
            }

            return null;
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Comment c = this.Service.GetComment(id);
            return View(c);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id, Content")] CommentBm bm)
        {
            try
            {
                await this.Service.Edit(bm);
                
                return new HttpStatusCodeResult(304);
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            try
            {
                await this.Service.DeleteAsync(id);
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }
    }
}
