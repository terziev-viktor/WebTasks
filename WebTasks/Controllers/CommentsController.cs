using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WebTasks.Models.EntityModels;
using WebTasks.Models.ViewModels;
using WebTasks.Services;

namespace WebTasks.Controllers
{
    public class CommentsController : Controller
    {
        private readonly CommentsService service = new CommentsService();

        // GET: Comments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Create(string Content, int ForTask, int TaskType)
        {
            if(Content == null)
            {
                return new HttpStatusCodeResult(403);
            }
            Content = Content.Trim();
            if(Content.Length == 0)
            {
                return new HttpStatusCodeResult(403);
            }
            Comment c = this.service.CreateComment(ForTask, Content, this.User.Identity.GetUserId());
            bool added = this.service.AddComment(c, ForTask, TaskType);
            if(added)
            {
                CommentVm vm = this.service.GetCommentVm(c);
                return PartialView("CommentPosted", vm);
            }

            return null;
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Comments/Delete/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            try
            {
                if (this.User.IsInRole("Admin"))
                await this.service.DeleteCommentAsync(id);
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(401);
            }
        }
    }
}
