using Maksimalist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maksimalist.Controllers
{
    public class HomeController : Controller
    {
        MaksimalistContext db = new MaksimalistContext();
        public ActionResult Index()
        {

            var manset = from s in db.Post
                         orderby s.PostDate descending
                         select s;

            var postlar = from person in db.Post
                        orderby person.PostDate descending
                        select person;
            
           
            ViewBag.manset = manset.Take(6);
            
            IEnumerable<Maksimalist.Models.Post> x = postlar.ToList();
            Advert ad = db.Advert.First();
            Post popular = db.Post.First();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Post = popular;
            ViewBag.RightNav = rn;
            return View(x);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}