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
    public class PostController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: Post
        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.Author).Include(p => p.Category);
            return View(post.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(string urlSlug, string kategori, string altKategori)
        {
        
           Post post = db.Post.Where(x => x.UrlSlug == urlSlug && x.Category.UrlSlug == kategori && x.SubCategory.UrlSlug == altKategori).FirstOrDefault();
          
            if (post == null)
            {
                return HttpNotFound();
            }

           
            Advert ad = db.Advert.First();
            Post popular = db.Post.First();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Post = popular;
            ViewBag.RightNav = rn;
            return View(post);
        }
        public ActionResult Galeri(string urlSlug, string kategori, string altKategori)
        {

            Post post = db.Post.Where(x => x.UrlSlug == urlSlug && x.Category.UrlSlug == kategori && x.SubCategory.UrlSlug == altKategori).FirstOrDefault();
            if (post == null)
            {
                return HttpNotFound();
            }
            Advert ad = db.Advert.First();
            Post popular = db.Post.First();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Post = popular;
            ViewBag.RightNav = rn;
            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            return View();
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
