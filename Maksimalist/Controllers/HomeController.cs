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
        public ActionResult Index()
        {
            HomeViewModel hmw = new HomeViewModel();
            hmw.Manset = db.Post.Where(x=> x.Manset==true).OrderByDescending(x => x.PostDate).Take(4).ToList();
            hmw.Moda = db.Post.Where(x => x.Category.UrlSlug == "Moda" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Guzellik = db.Post.Where(x => x.Category.UrlSlug == "Guzellik" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Alisveris = db.Post.Where(x => x.Category.UrlSlug == "Alışveriş" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.Unluler = db.Post.Where(x => x.Category.UrlSlug == "Ünlüler" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(3).ToList();
            hmw.SehirYasam = db.Post.Where(x => x.Category.UrlSlug == "Şehir-Yaşam" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(1).ToList();
            hmw.Gelin = db.Post.Where(x => x.Category.UrlSlug == "Gelin" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(1).ToList();
            hmw.Video = db.Post.Where(x => x.Category.UrlSlug == "Video" && x.Manset == false).OrderByDescending(x => x.PostDate).Take(3).ToList();
           
            
           
         
            
            
            Advert ad = db.Advert.First();
            Post popular = db.Post.First();
            RightNavViewModel rn = new RightNavViewModel();
            rn.Advert = ad;
            rn.Post = popular;
            ViewBag.RightNav = rn;
            return View(hmw);
        }

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