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
    public class CategoriesController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: Categories
        public ActionResult Details(string kategori)
        {
            ViewBag.CategoryName = kategori;
            if (kategori != null)
            {
                Category catgo = db.Category.Where(x => x.UrlSlug == kategori).FirstOrDefault();
                if (catgo != null)
                {
               
                    catgo.Posts = catgo.Posts.ToList();
                    return View(catgo.Posts);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return HttpNotFound();
        }

        // GET: Categories/Details/5
        public ActionResult Index(int? id)
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
