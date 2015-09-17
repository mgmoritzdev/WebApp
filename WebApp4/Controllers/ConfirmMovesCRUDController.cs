using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    public class ConfirmMovesCRUDController : Controller
    {
        private MyFileDbContext db = new MyFileDbContext();

        // GET: ConfirmMovesCRUD
        public ActionResult Index()
        {
            return View(db.ConfirmMoves.ToList());
        }

        // GET: ConfirmMovesCRUD/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfirmMove confirmMove = db.ConfirmMoves.Find(id);
            if (confirmMove == null)
            {
                return HttpNotFound();
            }
            return View(confirmMove);
        }

        // GET: ConfirmMovesCRUD/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfirmMovesCRUD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,myFileId,confirmed")] ConfirmMove confirmMove)
        {
            if (ModelState.IsValid)
            {
                db.ConfirmMoves.Add(confirmMove);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(confirmMove);
        }

        // GET: ConfirmMovesCRUD/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfirmMove confirmMove = db.ConfirmMoves.Find(id);
            if (confirmMove == null)
            {
                return HttpNotFound();
            }
            return View(confirmMove);
        }

        // POST: ConfirmMovesCRUD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,myFileId,confirmed")] ConfirmMove confirmMove)
        {
            if (ModelState.IsValid)
            {
                db.Entry(confirmMove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(confirmMove);
        }

        // GET: ConfirmMovesCRUD/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfirmMove confirmMove = db.ConfirmMoves.Find(id);
            if (confirmMove == null)
            {
                return HttpNotFound();
            }
            return View(confirmMove);
        }

        // POST: ConfirmMovesCRUD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConfirmMove confirmMove = db.ConfirmMoves.Find(id);
            db.ConfirmMoves.Remove(confirmMove);
            db.SaveChanges();
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
