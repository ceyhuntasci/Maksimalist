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
using Maksimalist.Areas.mmadmin.Models;

namespace Maksimalist.Areas.mmadmin.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: mmadmin/Gallery
        public ActionResult Index(string searchString)
        {
            var galleries = db.Gallery.OrderByDescending(x => x.Id).Take(50).ToList(); 
            if (!String.IsNullOrEmpty(searchString))
            {
                galleries = galleries.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
            } 
            return View(galleries);
        }

        // GET: mmadmin/Gallery/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: mmadmin/Gallery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public ActionResult Create(String GalleryName)
        {
            List<Matter> matterList = new List<Matter>();
            Gallery gallery = new Gallery();

            int j = 2;
            var tempName = Tools.toUrlSlug(GalleryName);
            gallery.Name = Tools.toUrlSlug(GalleryName);
            while (true)
            {

                if (db.Gallery.FirstOrDefault(x => x.Name == gallery.Name) == null)
                {
                    break;
                }
                else
                {
                    gallery.Name = tempName + "-" + j;
                    j++;
                }

            }
           

           
            for (int i = 0; i < Request.Files.Count; i++)
            {
                
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    Matter matter = new Matter();
                   

                    var fileName = Path.GetFileName(file.FileName);
                    matter.Name = fileName;

                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name));
                    file.SaveAs(path);
                    matter.Url = "/Images/Uploads/Galeri/" + gallery.Name + "/" + fileName;

                    matterList.Add(matter);
                  
                }
            }
            
            gallery.Matter = matterList;
            db.Gallery.Add(gallery);
            db.SaveChanges();
            
            return RedirectToAction("Index");
            
        }
        

        // GET: mmadmin/Gallery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: mmadmin/Gallery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
  
        public ActionResult Edit(int[] id, string[] Content)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < id.Count(); i++) {
                    var number = id[i];
                    var matter = db.Matter.First(c => c.Id == number);
                    matter.Content = Content[i];
                    matter.Order = i + 1;
        
                db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }
        [HttpPost]
        public ActionResult Add(String GalleryName)
        {
            GalleryName = Tools.toUrlSlug(GalleryName);
            Gallery gallery = db.Gallery.Where(x => x.Name == GalleryName).FirstOrDefault();
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    Matter matter = new Matter();


                    var fileName = Path.GetFileName(file.FileName);
                    matter.Name = fileName;

                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name), fileName);
                    if (!System.IO.Directory.Exists(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name));
                    }
                    
                    file.SaveAs(path);
                    matter.Url = "/Images/Uploads/Galeri/" + gallery.Name + "/" + fileName;

                    gallery.Matter.Add(matter);

                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: mmadmin/Gallery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: mmadmin/Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Gallery.Find(id);
            foreach(var m in gallery.Matter.ToList())
            {
                if (System.IO.File.Exists(Server.MapPath(m.Url)))
                {
                    System.IO.File.Delete(Server.MapPath(m.Url));
              
                  
                }
               
                db.Matter.Remove(m);
              
            }

            if (System.IO.Directory.Exists(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name)) && gallery.Name != "")
            {
                Directory.Delete(Server.MapPath("~/Images/Uploads/Galeri/" + gallery.Name)); 
            }
            if (db.Post.FirstOrDefault(m => m.GalleryId == gallery.Id) != null) 
            {
                db.Post.FirstOrDefault(m => m.GalleryId == gallery.Id).GalleryId = null;
            }
            db.Gallery.Remove(gallery);
            

            db.SaveChanges();
            return RedirectToAction("Index");
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
