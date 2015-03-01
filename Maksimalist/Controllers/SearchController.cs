using Maksimalist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace Maksimalist.Controllers
{
    public class SearchController : Controller
    {
        MaksimalistContext db = new MaksimalistContext();
        [OutputCache(Duration = 30)]
        public ActionResult Index(string s, string page)
        {
            if (String.IsNullOrEmpty(s))
            {
                return HttpNotFound();
            }

            ViewBag.Search = s;
            
           
            List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(5).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.GununObjesi = db.Post.Where(x => x.SubCategory.UrlSlug == "GununObjesi" && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
           
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Page = 1;
            ViewBag.Title = s;
            List<Post> posts = new List<Post>();
            List<Post> postCollection = new List<Post>();
            if (!String.IsNullOrEmpty(s))
            {
               
                postCollection = db.Post.Where(x => x.Tags.Any(c => c.Name.ToUpper().Contains(s)) || x.Headline.ToUpper().Contains(s)).ToList();
                double ceiling = (double)postCollection.Count() / (double)5;
                ViewBag.PageCount = Math.Ceiling(ceiling);
                posts = postCollection.Take(5).ToList();

            }
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
            if(Request.Browser.IsMobileDevice){
                return View("MobileSearch",posts);
            }
            return View(posts);


        }
    }
}