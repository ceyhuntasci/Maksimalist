using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Maksimalist
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Anasayfa",
                 url: "",
                 defaults: new { controller = "Home", action = "About" },
                 namespaces: new[] { "Maksimalist.Controllers" }
             );
            routes.MapRoute(
               name: "Kategori",
               url: "kategori/{kategori}/{altkategori}",
               defaults: new { controller = "Categories", action = "Details" , altkategori=""},
               namespaces: new[] { "Maksimalist.Controllers" },
               constraints: new { kategori = "(?!anan).*" } 
           );
         
            routes.MapRoute(
               name: "Haber",
               url: "post/{kategori}/{altKategori}/{urlSlug}",
               defaults: new { controller = "Post", action = "Details" },
               namespaces: new[] { "Maksimalist.Controllers" }
           );
            routes.MapRoute(
               name: "Search",
               url: "{controller}",
               defaults: new { controller = "Search", action = "Index" },
               namespaces: new[] { "Maksimalist.Controllers" }
           );
            routes.MapRoute(
              name: "Galeri",
              url: "post/galeri/{kategori}/{altKategori}/{urlSlug}",
              defaults: new { controller = "Post", action = "Galeri" },
              namespaces: new[] { "Maksimalist.Controllers" }
              );

            routes.MapRoute(
                 name: "He Şu",
                 url: "anasayfa/index/",
                 defaults: new { controller = "Home", action = "Index" },
                 namespaces: new[] { "Maksimalist.Controllers" }
                 );
            routes.MapRoute(
                 name: "getresult",
                 url: "getResult/{email}/kaydet",
                 defaults: new { controller = "Home", action = "GetResult" },
                 namespaces: new[] { "Maksimalist.Controllers" }
                 );

        }
    }
}
