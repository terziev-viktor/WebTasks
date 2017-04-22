using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTasks.Models;
using WebTasks.Models.EntityModels;

namespace WebTasks.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/DailyTasks
        public async Task<ActionResult> Index()
        {
            return View(await db.DailyTasks.ToListAsync());
        }

        // GET: Admin/DailyTasks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await db.DailyTasks.FindAsync(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // GET: Admin/DailyTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DailyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Note,Deadline,Title,Description")] DailyTask dailyTask)
        {
            if (ModelState.IsValid)
            {
                db.DailyTasks.Add(dailyTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dailyTask);
        }

        // GET: Admin/DailyTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await db.DailyTasks.FindAsync(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: Admin/DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Note,Deadline,Title,Description")] DailyTask dailyTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dailyTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dailyTask);
        }

        // GET: Admin/DailyTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTask dailyTask = await db.DailyTasks.FindAsync(id);
            if (dailyTask == null)
            {
                return HttpNotFound();
            }
            return View(dailyTask);
        }

        // POST: Admin/DailyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DailyTask dailyTask = await db.DailyTasks.FindAsync(id);
            db.DailyTasks.Remove(dailyTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
