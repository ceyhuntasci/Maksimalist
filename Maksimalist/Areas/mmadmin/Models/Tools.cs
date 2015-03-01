using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maksimalist.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace Maksimalist.Areas.mmadmin.Models
{
    public static class Tools
    {


        public static string toUrlSlug(string turkish)
        {
            string urlSlug = turkish.Replace("ı", "i");
            urlSlug = urlSlug.Replace("İ", "I");
            urlSlug = urlSlug.Replace(" ", "-");
            urlSlug = urlSlug.Replace("ö", "o");
            urlSlug = urlSlug.Replace("ç", "c");
            urlSlug = urlSlug.Replace("ü", "u");
            urlSlug = urlSlug.Replace("ş", "s");
            urlSlug = urlSlug.Replace("ğ", "g");
            urlSlug = urlSlug.Replace("Ö", "O");
            urlSlug = urlSlug.Replace("Ç", "C");
            urlSlug = urlSlug.Replace("Ü", "U");
            urlSlug = urlSlug.Replace("Ş", "S");
            urlSlug = urlSlug.Replace("Ğ", "G");
           
            urlSlug = Regex.Replace(urlSlug, "[^0-9a-zA-Z-]+","");
            return urlSlug;
        }
        public static String SaveFile(HttpPostedFileBase file1, Post post)
        {
            string UrlFeed = "";
            if (file1 != null && file1.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file1.FileName);
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);

                while (File.Exists(path))
                {
                    fileName = Path.GetFileNameWithoutExtension(file1.FileName) + "-" + Path.GetExtension(file1.FileName);
                    path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);
                }


                file1.SaveAs(path);
                UrlFeed = "/Images/Uploads/" + post.UrlSlug + "/" + fileName;

            }
            return UrlFeed;
        }
        public static String SaveContentImages(Post post)
        {
            string ContentFeed = post.Content;
            if (Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/Images/Temp")) != null)
            {
                string[] tempfiles = System.IO.Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/Images/Temp"));
                foreach (string s in tempfiles)
                {
                    var fileName = Path.GetFileName(s);
                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);

                    while (File.Exists(path))
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileName) + "-" + Path.GetExtension(fileName);
                        path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);
                    }
                    if(!File.Exists(path)){
                         System.IO.File.Move(s, path);

                    }
                   
                }
                ContentFeed = post.Content.Replace("Images/Temp", "Images/Uploads/" + post.UrlSlug);


            }
            return ContentFeed;
        }
        public static void MoveFolder(Post post, string oldUrlSlug)
        {
            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug));
            string[] tempfiles = System.IO.Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + oldUrlSlug));
            foreach (string s in tempfiles)
            {
                System.IO.File.Move(s, Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + post.UrlSlug), Path.GetFileName(s)));

            }
            Directory.Delete(System.Web.HttpContext.Current.Server.MapPath("~/Images/Uploads/" + oldUrlSlug));

        }

    }
}