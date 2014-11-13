using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maksimalist.Models;

namespace Maksimalist.Controllers
{
    public class MatterController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: Matter
        public ActionResult Index()
        {
            var matter = db.Matter.Include(m => m.Gallery);
            return View(matter.ToList());
        }

        // GET: Matter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matter matter = db.Matter.Find(id);
            if (matter == null)
            {
                return HttpNotFound();
            }
            return View(matter);
        }

        // GET: Matter/Create
        public ActionResult Create()
        {
            ViewBag.GalleryId = new SelectList(db.Gallery, "Id", "Name");
            return View();
        }
        
        // POST: Matter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GalleryId,Url,Content")] Matter matter)
        {
            if (ModelState.IsValid)
            {
                db.Matter.Add(matter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GalleryId = new SelectList(db.Gallery, "Id", "Name", matter.GalleryId);
            return View(matter);
        }

        // GET: Matter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matter matter = db.Matter.Find(id);
            if (matter == null)
            {
                return HttpNotFound();
            }
            ViewBag.GalleryId = new SelectList(db.Gallery, "Id", "Name", matter.GalleryId);
            return View(matter);
        }

        // POST: Matter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GalleryId,Url,Content")] Matter matter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GalleryId = new SelectList(db.Gallery, "Id", "Name", matter.GalleryId);
            return View(matter);
        }

        // GET: Matter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matter matter = db.Matter.Find(id);
            if (matter == null)
            {
                return HttpNotFound();
            }
            return View(matter);
        }

        // POST: Matter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matter matter = db.Matter.Find(id);
            db.Matter.Remove(matter);
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
