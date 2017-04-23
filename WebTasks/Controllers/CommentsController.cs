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
        public ActionResult Create(string Content, int ForTask)
        {
            Comment c = this.service.CreateComment(ForTask, Content, this.User.Identity.Name);
            bool added = this.service.AddComment(c, ForTask);
            if(added)
            {
                this.service.SaveChanges();
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

        // GET: Comments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
