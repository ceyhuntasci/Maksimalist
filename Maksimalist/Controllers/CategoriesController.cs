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

         [OutputCache(Duration = 30)]
        public ActionResult Details(string kategori, string page, string altkategori)
        {
            
           
            ViewBag.Page = 1;
            if (kategori != null)
            {

                List<Post> posts = new List<Post>();
                List<Post> postCollection = new List<Post>();
                Category catgo = db.Category.Where(x => x.UrlSlug == kategori).FirstOrDefault();

             

                if (catgo != null)
                {
                    ViewBag.CategoryName = catgo.Name;
                    postCollection = catgo.Posts.Where(x => x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).ToList();
                    
                    if (altkategori != null)
                    {
                        SubCategory subcat = catgo.SubCategory.Where(x => x.UrlSlug == altkategori).FirstOrDefault();
                        if (subcat != null)
                        {
                            postCollection = subcat.Posts.ToList();
                            ViewBag.CategoryName = subcat.Name;
                        }
                        
                    }
                    
                    double ceiling = (double)postCollection.Count() / (double)5;
                    ViewBag.PageCount = Math.Ceiling(ceiling);
                    posts = postCollection.Take(5).ToList();

                    if (!String.IsNullOrEmpty(page))
                    {
                        int p = 0;
                        if (Int32.TryParse(page, out p))
                        {
                            ViewBag.Page = p;
                            p = p - 1;
                            p = p * 5;
                            posts = postCollection.Skip(p).Take(5).ToList();

                        }
                        else
                        {
                            return HttpNotFound();
                        }

                    }

                   
           
                    List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(5).ToList();
                    popular.OrderBy(x => x.HitCount).ToList();
                    RightNavViewModel rn = new RightNavViewModel();
                    rn.GununObjesi = db.Post.Where(x => x.SubCategory.UrlSlug == "GununObjesi" && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
           
                    rn.Posts = popular;
                    ViewBag.Title = ViewBag.CategoryName;
                    ViewBag.RightNav = rn;

                    
                    return View(posts);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return HttpNotFound();
        }

         [OutputCache(Duration = 30)]
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
