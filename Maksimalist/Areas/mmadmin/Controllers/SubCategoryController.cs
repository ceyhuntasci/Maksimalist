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
    [Authorize]
    public class SubCategoryController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: mmadmin/SubCategory
        public ActionResult Index()
        {
            var subCategory = db.SubCategory.Include(s => s.Category);
            return View(subCategory.ToList());
        }

        // GET: mmadmin/SubCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: mmadmin/SubCategory/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: mmadmin/SubCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                subCategory.UrlSlug = toUrlSlug(subCategory.Name);
                db.SubCategory.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: mmadmin/SubCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: mmadmin/SubCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: mmadmin/SubCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: mmadmin/SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.SubCategory.Find(id);
            var posts = db.Post.Where(x => x.SubCategory == subCategory).ToList();
            foreach (var post in posts)
            {
                post.SubCategory = null;
            }
            db.SubCategory.Remove(subCategory);
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
            urlSlug = urlSlug.Replace("İ", "i");
            urlSlug = urlSlug.Replace(" ", "-");
            urlSlug = urlSlug.Replace("ö", "o");
            urlSlug = urlSlug.Replace("ç", "c");
            urlSlug = urlSlug.Replace("ü", "u");
            urlSlug = urlSlug.Replace("ş", "s");
            urlSlug = urlSlug.Replace("ğ", "g");
            urlSlug = urlSlug.Replace("Ö", "o");
            urlSlug = urlSlug.Replace("Ç", "c");
            urlSlug = urlSlug.Replace("Ü", "u");
            urlSlug = urlSlug.Replace("Ş", "s");
            urlSlug = urlSlug.Replace("Ğ", "g");
            return urlSlug;
        }
    }
}
