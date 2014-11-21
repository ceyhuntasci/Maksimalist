using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maksimalist.Models;

namespace Maksimalist.Areas.mmadmin.Controllers
{
    public class AdvertController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: mmadmin/Advert
        public ActionResult Index()
        {
            return View(db.Advert.ToList());
        }

        // GET: mmadmin/Advert/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Advert.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // GET: mmadmin/Advert/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mmadmin/Advert/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Url")] Advert advert)
        {
            if (ModelState.IsValid)
            {
                db.Advert.Add(advert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(advert);
        }

        // GET: mmadmin/Advert/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Advert.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // POST: mmadmin/Advert/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Url")] Advert advert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(advert);
        }

        // GET: mmadmin/Advert/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Advert.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // POST: mmadmin/Advert/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert advert = db.Advert.Find(id);
            db.Advert.Remove(advert);
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
