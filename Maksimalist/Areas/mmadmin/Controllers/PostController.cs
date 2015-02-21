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
        public ActionResult Index(string searchString)
        {

            var posts = db.Post.Include(p => p.Author).Include(p => p.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Tags.Any(c => c.Name.Contains(searchString)) || s.Headline.Contains(searchString));
                posts.Include(p => p.Author).Include(p => p.Category);
                return View(posts);
            }
            posts = posts.OrderByDescending(x => x.Id).Take(25);
            return View(posts);


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
            ViewBag.SubCategoryId = new SelectList(db.Category.FirstOrDefault().SubCategory.ToList(), "Id", "Name");
            ViewBag.Tags = new List<String>();

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,CategoryId,AuthorId,GalleryId,SubCategoryId,Headline,Bottomline,Content,PostDate,HasGallery,Manset,HasVideo,VideoUrl")] Post post, HttpPostedFileBase file1, HttpPostedFileBase file2, string[] Tags)
        {

            if (ModelState.IsValid)
            {
                int j = 2;
                var tempName = post.Headline;
                while (true)
                {

                    if (db.Post.FirstOrDefault(x => x.Headline == post.Headline) == null)
                    {
                        break;
                    }
                    else
                    {
                        post.Headline = tempName + "-" + j;
                        j++;
                    }

                }
                post.UrlSlug = toUrlSlug(post.Headline);
                if (file1 != null && file1.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file1.FileName);



                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + post.UrlSlug));
                    file1.SaveAs(path);
                    post.ImageUrl = "/Images/Uploads/" + post.UrlSlug + "/" + fileName;

                    if (Directory.GetFiles(Server.MapPath("~/Images/Temp")) != null)
                    {
                        string[] tempfiles = System.IO.Directory.GetFiles(Server.MapPath("~/Images/Temp"));
                        foreach (string s in tempfiles)
                        {
                            System.IO.File.Move(s, Path.Combine(Server.MapPath("~/Images/Uploads/" + post.UrlSlug), Path.GetFileName(s)));


                        }
                        post.Content = post.Content.Replace("Images/Temp", "Images/Uploads/" + post.UrlSlug);
                    }

                }
                if (file2 != null && file2.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file2.FileName);



                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/" + post.UrlSlug), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + post.UrlSlug));
                    file2.SaveAs(path);
                    post.ContentImage = "/Images/Uploads/" + post.UrlSlug + "/" + fileName;

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
                post.HitCount = 0;

                db.Post.Add(post);

                db.SaveChanges();



                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            ViewBag.SubCategoryId = new SelectList(db.Category.FirstOrDefault().SubCategory.ToList(), "Id", "Name");

            return View(post);
        }

        [HttpGet]
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
            ViewBag.SubCategoryId = new SelectList(db.SubCategory.Where(x => x.Name == post.SubCategory.Name).ToList(), "Id", "Name");
            if (post.HasGallery)
            {
                ViewBag.HasGallery = "checked";
            }
            if (post.HasVideo)
            {
                ViewBag.HasVideo = "checked";
            }
            if (post.Manset)
            {
                ViewBag.Manset = "checked";
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit([Bind(Include = "Id,CategoryId,AuthorId,GalleryId,HitCount,SubCategoryId,Headline,Bottomline,Content,PostDate,HasGallery,Manset,HasVideo,VideoUrl")] Post post, HttpPostedFileBase file1, HttpPostedFileBase file2, string[] Tags)
        {
            Post post2 = db.Post.Find(post.Id);
            if (ModelState.IsValid)
            {
               
                post2.AuthorId = post.AuthorId;
                post2.Bottomline = post.Bottomline;
                post2.CategoryId = post.CategoryId;
                post2.Content = post.Content;
                post2.GalleryId = post.GalleryId;
                post2.HasGallery = post.HasGallery;
                post2.HasVideo = post.HasVideo;
                post2.HitCount = post.HitCount;
                post2.Manset = post.Manset;
                post2.PostDate = post.PostDate;
                post2.SubCategoryId = post.SubCategoryId;
                post2.VideoUrl = post.VideoUrl;

                if (post2.Headline != post.Headline)
                {
                    int j = 2;
                    var tempName = post.Headline;
                    while (true)
                    {

                        if (db.Post.FirstOrDefault(x => x.Headline == post.Headline) == null)
                        {
                            break;
                        }
                        else
                        {
                            post.Headline = tempName + "-" + j;
                            j++;
                        }

                    }
                }
               

                string oldUrlSlug = post2.UrlSlug;
                post2.UrlSlug = toUrlSlug(post.Headline);
                #region FileControl
                if (post2.Headline != post.Headline)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug));
                    string[] tempfiles = System.IO.Directory.GetFiles(Server.MapPath("~/Images/Uploads/" + oldUrlSlug));
                    foreach (string s in tempfiles)
                    {
                        System.IO.File.Move(s, Path.Combine(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug), Path.GetFileName(s)));

                    }
                    Directory.Delete(Server.MapPath("~/Images/Uploads/" + oldUrlSlug));
                    post2.ImageUrl = post2.ImageUrl.Replace(oldUrlSlug, post2.UrlSlug);
                    post2.ContentImage = post2.ContentImage.Replace(oldUrlSlug, post2.UrlSlug);
                    post2.Content = post2.Content.Replace("Images/Uploads/" + oldUrlSlug, "Images/Uploads/" + post2.UrlSlug);
                }
               

                if (file1 != null && file1.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file1.FileName);



                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug));
                    file1.SaveAs(path);
                    post2.ImageUrl = "/Images/Uploads/" + post2.UrlSlug + "/" + fileName;

                    

                }
                if (Directory.GetFiles(Server.MapPath("~/Images/Temp")) != null)
                {
                    string[] tempfiles = System.IO.Directory.GetFiles(Server.MapPath("~/Images/Temp"));
                    foreach (string s in tempfiles)
                    {
                        System.IO.File.Move(s, Path.Combine(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug), Path.GetFileName(s)));


                    }
                    post2.Content = post2.Content.Replace("Images/Temp", "Images/Uploads/" + post2.UrlSlug);
                }
                
                if (file2 != null && file2.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file2.FileName);



                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/Images/Uploads/" + post2.UrlSlug));
                    file2.SaveAs(path);
                    post2.ContentImage = "/Images/Uploads/" + post2.UrlSlug + "/" + fileName;

                } 
                #endregion
                post2.Headline = post.Headline;
                #region TagControl
                foreach (var i in post2.Tags)
                {
                    if (!Tags.Contains(i.Name))
                    {
                        i.Posts.Remove(post2);
                    }
                }
                foreach (var i in Tags)
                {


                    Tag x = new Tag();
                    x.Name = i;
                    List<Post> postList = new List<Post>();
                    List<Tag> tagList = new List<Tag>();

                    postList.Add(post2);

                    if (db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault() != null && post2.Tags.Where(lin => lin.Name == x.Name).FirstOrDefault() != null)
                    {

                    }

                    if (db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault() != null && post2.Tags.Where(lin => lin.Name == x.Name).FirstOrDefault() == null)
                    {
                        x = db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault();

                        post2.Tags.Add(x);

                    }
                    else if (db.Tag.Where(linq => linq.Name == x.Name).FirstOrDefault() == null && post2.Tags.Where(lin => lin.Name == x.Name).FirstOrDefault() == null)
                    {
                        x.Posts = postList;
                        db.Tag.Add(x);
                    }

                } 
                #endregion

                db.Entry(post2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Author, "Id", "FirstName", post.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", post.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.Category.FirstOrDefault().SubCategory.ToList(), "Id", "Name");
            return View(post2);
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
            if (System.IO.Directory.Exists(Server.MapPath("~/Images/Uploads/" + post.UrlSlug)))
            {
                System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Server.MapPath("~/Images/Uploads/" + post.UrlSlug));

                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(Server.MapPath("~/Images/Uploads/" + post.UrlSlug));
            }
            if (post.Tags.ToList().FirstOrDefault() != null)
            {
                foreach (var i in post.Tags.ToList())
                {
                    i.Posts.Remove(post);
                    if (i.Posts.FirstOrDefault() == null)
                    {
                        db.Tag.Remove(i);
                    }
                } 
            }

            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void uploadnow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Temp"));
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/Temp"), ImageName);
                upload.SaveAs(path);
            }
        }
        public ActionResult uploadPartial()
        {
            var appData = Server.MapPath("~/Images/Temp");
            var images = Directory.GetFiles(appData).Select(x => new imagesviewmodel
            {
                Url = Url.Content("/Images/Temp/" + Path.GetFileName(x))
            });
            return View(images);
        }
        public ActionResult GetSubCategories(int id)
        {
            List<SelectListItem> subcategories = new List<SelectListItem>();
            //The below code is hardcoded for demo. you mat replace with DB data 
            //based on the  input coming to this method ( product id)
            foreach (var z in db.Category.Find(id).SubCategory.ToList())
            {
                SelectListItem item = new SelectListItem { Value = z.Id.ToString(), Text = z.Name };
                subcategories.Add(item);
            }

            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string toUrlSlug(string turkish)
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
            return urlSlug;
        }
    }
}
