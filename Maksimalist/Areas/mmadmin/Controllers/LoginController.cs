using Maksimalist.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Maksimalist.Areas.mmadmin.Controllers
{

    public class LoginController : Controller
    {
        // GET: mmadmin/Login
        private MaksimalistContext db = new MaksimalistContext();

        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(Author author)
        {
            if (ModelState.IsValid)
            {
                author.Password = GenerateMD5(author.Password);
                Author canım = db.Author.Where(m => m.UserName == author.UserName && m.Password == author.Password).FirstOrDefault();
                if (canım != null)
                {
                   

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, author.UserName));
                    claims.Add(new Claim(ClaimTypes.GivenName, canım.FirstName));
                    claims.Add(new Claim(ClaimTypes.Sid, canım.Id.ToString()));
                    var id = new ClaimsIdentity(claims,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                   
                   
                    var authenticationManager = Request.GetOwinContext().Authentication;
                    authenticationManager.SignIn(id);

                    
                   authenticationManager.User.AddIdentity(id);


                   return RedirectToAction("Index", "Main");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }

            }
            return View(author);
        }

        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public string GenerateMD5(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}