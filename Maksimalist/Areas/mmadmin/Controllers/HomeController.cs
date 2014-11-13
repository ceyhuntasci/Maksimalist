using Maksimalist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Maksimalist.Areas.mmadmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();
        
        public ActionResult Index()
        {
            ViewBag.AuthorFirstName = Session["author"];
            return View();
        }
    
    }
}