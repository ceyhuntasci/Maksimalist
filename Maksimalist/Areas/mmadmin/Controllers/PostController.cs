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
    [Authorize]
    public class PostController : Controller
    {
        private MaksimalistContext db = new MaksimalistContext();
    
        
        // GET: Posts
        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.Author).Include(p => p.Category);
            return View(post.ToList());

        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create

        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            ViewBag.SubCategoryId = new SelectList(db.SubCategory, "Id", "Name");
            ViewBag.Tags = new List<String>();

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,CategoryId,AuthorId,SubCategoryId,GalleryId,Headline,Bottomline,Content,UrlSlug,PostDate,HasGallery,Manset")] Post post,HttpPostedFileBase file, string[] Tags)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
            
                    var fileName = Path.GetFileName(file.FileName);
                   
                   
                 
                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/"+post.Headline), fileName);
                   
                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/"+post.Headline));
                    file.SaveAs(path);
                    post.ImageUrl = "/Images/Uploads/"+post.Headline+"/"+fileName;

                }
               
                foreach (var i in Tags)
                {
                    Tag x = new Tag();
                    x.Name = i;
                    List<Post> tagList = new List<Post>();
                    List<Tag> postList = new List<Tag>();
                   
                    tagList.Add(post);
                    x.Posts = tagList;
                    if (db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault() != null)
                    {
                        x = db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault();
                        postList.Add(x);
                        post.Tags = postList;
                       
                    }

                    else
                    {
                        db.Tag.Add(x);
                    }

                }


                db.Post.Add(post);
                db.SaveChanges();



                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", post.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategory, "Id", "Name", post.AuthorId);

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,AuthorId,Headline,Bottomline,Content,UrlSlug,ImageUrl,PostDate,Manset")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
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
