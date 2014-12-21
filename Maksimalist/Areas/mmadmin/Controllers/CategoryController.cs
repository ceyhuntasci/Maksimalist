using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maksimalist.Models;
using System.Web.Security;

namespace Maksimalist.Areas.mmadmin.Controllers
{
    public class CategoryController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: mmadmin/Category
        public ActionResult Index()
        {
            return View(db.Category.ToList());
            
        }

        // GET: mmadmin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: mmadmin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mmadmin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.UrlSlug = toUrlSlug(category.Name);
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: mmadmin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: mmadmin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: mmadmin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: mmadmin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            var posts = db.Post.Where(x => x.Category.Name == category.Name).ToList();
            foreach(var post in posts){
                 post.Category = db.Category.Where(cat => cat.Name == "Bos").FirstOrDefault(); 
            }
            db.Category.Remove(category);
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
        public string toUrlSlug(string turkish)
        {
            string urlSlug = turkish.Replace("ı", "i");
            urlSlug = turkish.Replace(" ", "");
            urlSlug = turkish.Replace("ö", "o");
            urlSlug = turkish.Replace("ç", "c");
            urlSlug = turkish.Replace("ü", "u");
            urlSlug = turkish.Replace("ş", "s");
            urlSlug = turkish.Replace("ğ", "g");
            return urlSlug;
        }
    }
}
