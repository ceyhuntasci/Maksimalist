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

        [OutputCache(Duration = 30)]
        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.Author).Include(p => p.Category);
            return View(post.ToList());
        }

         [OutputCache(Duration = 30)]
        public ActionResult Details(string urlSlug, string kategori, string altKategori)
        {
        
           Post post = db.Post.Where(x => x.UrlSlug == urlSlug && x.Category.UrlSlug == kategori && x.SubCategory.UrlSlug == altKategori).FirstOrDefault();
          
           
            if (post == null)
            {
                return HttpNotFound();
            }
        
            post.HitCount = post.HitCount + 1;
            db.SaveChanges();
           
            Advert ad = db.Advert.First();
            List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(3).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Title = post.Headline;
            ViewBag.Image = post.ImageUrl;
            ViewBag.last3post = db.Post.Where(x => x.Author.UserName == post.Author.UserName).OrderByDescending(x => x.PostDate).Take(3).ToList();
            return View(post);
        }
         [OutputCache(Duration = 30)]
        public ActionResult Galeri(string urlSlug, string kategori, string altKategori)
        {

            Post post = db.Post.Where(x => x.UrlSlug == urlSlug && x.Category.UrlSlug == kategori && x.SubCategory.UrlSlug == altKategori).FirstOrDefault();
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Gallery.Matter = post.Gallery.Matter.OrderByDescending(x => x.Order).ToList();

            Advert ad = db.Advert.First();
            List<Post> popular = db.Post.OrderBy(x => x.HitCount).Take(3).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Title = post.Headline;
            ViewBag.Image = post.ImageUrl;
            return View(post);
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
