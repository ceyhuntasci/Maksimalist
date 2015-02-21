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

            if (Request.Browser.IsMobileDevice)
            {
                return View("MobilePostDetail", post);
            }
           
            List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(3).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.GununObjesi = db.Post.Where(x => x.SubCategory.UrlSlug == "GununObjesi" && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
           
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Title = post.Headline;
            ViewBag.Image = post.ContentImage;
            ViewBag.Desc = post.Bottomline;
            
            ViewBag.last3post = db.Post.Where(x => x.Author.UserName == post.Author.UserName).OrderByDescending(x => x.PostDate).Take(3).ToList();
            #region BenzerPost
            List<Post> benzerpost = new List<Post>();

            foreach (var item in post.Tags.ToList())
            {

                benzerpost.AddRange(db.Post.Where(x => x.Tags.Any(c => c.Name == item.Name) && x.UrlSlug != post.UrlSlug).ToList());
                benzerpost = benzerpost.Distinct().ToList();
            }
            int i = 0;
            List<Post> benzerSub = db.Post.Where(x => x.SubCategory.Name == post.SubCategory.Name && x.UrlSlug != post.UrlSlug).OrderByDescending(x => x.HitCount).ToList();
            if (benzerSub.Count < 3)
            {
                benzerSub.AddRange(db.Post.Where(x => x.Category.Name == post.Category.Name && x.UrlSlug != post.UrlSlug).OrderByDescending(x => x.HitCount).ToList());
            }
            benzerSub = benzerSub.Distinct().ToList();
            benzerpost.AddRange(benzerSub);
 
            benzerpost = benzerpost.Distinct().Take(3).ToList();
            
            ViewBag.Benzer = benzerpost; 
            #endregion
             
   
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
            post.HitCount = post.HitCount + 1;
            post.Gallery.Matter = post.Gallery.Matter.OrderByDescending(x => x.Order).ToList();

            if(Request.Browser.IsMobileDevice){
                return View("MobileGaleri" ,post);
            }
           
            List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(5).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.GununObjesi = db.Post.Where(x => x.SubCategory.UrlSlug == "GununObjesi" && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
           
           
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Title = post.Headline;
            ViewBag.Image = post.ContentImage;
            ViewBag.Desc = post.Bottomline;
            ViewBag.last3post = db.Post.Where(x => x.Author.UserName == post.Author.UserName).OrderByDescending(x => x.PostDate).Take(3).ToList();
           
            #region BenzerPost
            List<Post> benzerpost = new List<Post>();

            foreach (var item in post.Tags.ToList())
            {

                benzerpost.AddRange(db.Post.Where(x => x.Tags.Any(c => c.Name == item.Name) && x.UrlSlug != post.UrlSlug).ToList());
                benzerpost = benzerpost.Distinct().ToList();
            }
            int i = 0;
            List<Post> benzerSub = db.Post.Where(x => x.SubCategory.Name == post.SubCategory.Name && x.UrlSlug != post.UrlSlug).OrderByDescending(x => x.HitCount).ToList();
            if (benzerSub.Count < 3)
            {
                benzerSub.AddRange(db.Post.Where(x => x.Category.Name == post.Category.Name && x.UrlSlug != post.UrlSlug).OrderByDescending(x => x.HitCount).ToList());
            }
            benzerSub = benzerSub.Distinct().ToList();
            benzerpost.AddRange(benzerSub);

            benzerpost = benzerpost.Distinct().Take(3).ToList();
           
            ViewBag.Benzer = benzerpost;
            #endregion
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
