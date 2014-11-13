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

namespace Maksimalist.Areas.mmadmin.Controllers
{
    
    public class GalleryController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();

        // GET: mmadmin/Gallery
        public ActionResult Index()
        {
            return View(db.Gallery.ToList());
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
            gallery.Name = GalleryName;

           
            for (int i = 0; i < Request.Files.Count; i++)
            {
                
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    Matter matter = new Matter();
                   

                    var fileName = Path.GetFileName(file.FileName);


                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/" + gallery.Name), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + gallery.Name));
                    file.SaveAs(path);
                    matter.Url = "/Images/Uploads/" + gallery.Name + "/" + fileName;

                    matterList.Add(matter);
                  
                }
            }
            
            gallery.Matter = matterList;
            db.Gallery.Add(gallery);
            db.SaveChanges();

            return RedirectToAction("Edit",gallery);
            
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
                    db.Matter.First(c => c.Id == number).Content = Content[i];
        
                db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
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
                db.Matter.Remove(m);
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
