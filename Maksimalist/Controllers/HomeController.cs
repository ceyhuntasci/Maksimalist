using Maksimalist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Maksimalist.Controllers
{
    public class HomeController : Controller
    {
        MaksimalistContext db = new MaksimalistContext();
        [OutputCache(Duration=30)]
        public ActionResult Index()
        {
            if(Request.Browser.IsMobileDevice){
                MobileHomeViewModel mhmw = new MobileHomeViewModel();
               mhmw.Postlar = db.Post.Where(x=> x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(15).ToList();
            

                return View("MobileHome",mhmw);
            }
            HomeViewModel hmw = new HomeViewModel();
            hmw.Manset = db.Post.Where(x=> x.Manset==true && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(4).ToList();
            hmw.Moda = db.Post.Where(x => x.Category.UrlSlug == "Moda" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Guzellik = db.Post.Where(x => x.Category.UrlSlug == "Guzellik" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Alisveris = db.Post.Where(x => x.Category.UrlSlug == "Alisveris" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Unluler = db.Post.Where(x => x.Category.UrlSlug == "Unluler" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.SehirYasam = db.Post.Where(x => x.Category.UrlSlug == "Sehir-Yasam" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
            hmw.Gelin = db.Post.Where(x => x.Category.UrlSlug == "Gelin" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
            hmw.Video = db.Post.Where(x => x.Category.UrlSlug == "Video" && x.Manset == false && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(3).ToList();
           
            
           
         
            
            
            List<Post> ad = db.Post.Take(2).ToList();
            List<Post> popular = db.Post.OrderByDescending(x => x.HitCount).Take(5).ToList();
            popular.OrderBy(x => x.HitCount).ToList();
            RightNavViewModel rn = new RightNavViewModel();
            rn.GununObjesi = db.Post.Where(x => x.SubCategory.UrlSlug == "GununObjesi" && x.PostDate <= DateTime.Now).OrderByDescending(x => x.PostDate).Take(1).ToList();
           
            rn.Adverts = ad;
            rn.Posts = popular;
            ViewBag.RightNav = rn;
            ViewBag.Title = "Maksimalist";
            ViewBag.Desc = "Moda, güzellik, ünlüler, alışveriş ve şehir hayatına Maksimalist bir bakış açısı getiriyoruz! Eğlenceli, akıcı, esprili; yeni nesil moda platformu.";
            
            return View(hmw);
        }
         [OutputCache(Duration = 30)]
        public ActionResult About(string Email)
        {
            return View();

            
        }
        public ActionResult GetResult(string Email)
        {
            try
            {
                MailAddress m = new MailAddress(Email);
                EmailGrubu em = new EmailGrubu();
                em.EmailUrl = Email;


                if (db.EmailGrubu.Where(x => x.EmailUrl == Email).FirstOrDefault() == null)
                {
                    db.EmailGrubu.Add(em);
                    db.SaveChanges();
                    return Json("Kaydoldunuz", JsonRequestBehavior.AllowGet);
                }
                else if (db.EmailGrubu.Where(x => x.EmailUrl == Email).FirstOrDefault() != null)
                {
                    return Json("Zaten kayıtlısınız", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Bir hata oluştu", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (FormatException)
            {
                return Json("Geçersiz Email", JsonRequestBehavior.AllowGet); ;
            }
           
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}