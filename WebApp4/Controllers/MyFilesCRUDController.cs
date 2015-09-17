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
    public class MyFilesCRUDController : Controller
    {
        private MyFileDbContext db = new MyFileDbContext();

        // GET: MyFilesCRUD
        public ActionResult Index()
        {
            return View(db.MyFiles.ToList());
        }

        // GET: MyFilesCRUD/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // GET: MyFilesCRUD/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyFilesCRUD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,dir,name,size")] MyFile myFile)
        {
            if (ModelState.IsValid)
            {
                db.MyFiles.Add(myFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myFile);
        }

        // GET: MyFilesCRUD/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // POST: MyFilesCRUD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,dir,name,size")] MyFile myFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myFile);
        }

        // GET: MyFilesCRUD/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyFile myFile = db.MyFiles.Find(id);
            if (myFile == null)
            {
                return HttpNotFound();
            }
            return View(myFile);
        }

        // POST: MyFilesCRUD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyFile myFile = db.MyFiles.Find(id);
            db.MyFiles.Remove(myFile);
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
