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
        public ActionResult Index(string s)
        {
            ViewBag.Search = s;
            var posts = from m in db.Post
                        select m;
            posts = db.Post.Include(au => au.Author).Include(p => p.Category);
            if (!String.IsNullOrEmpty(s))
            {
                posts = posts.Where(x=> x.Tags.Any(c => c.Name.ToUpper().Contains(s)) || x.Headline.ToUpper().Contains(s));
                posts.Include(p => p.Author).Include(p => p.Category);
            }
            return View(posts);
  
        }
    }
}