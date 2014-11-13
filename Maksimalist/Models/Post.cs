using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maksimalist.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int AuthorId { get; set; }
        
        public int? GalleryId { get; set; }
        public string Headline { get; set; }
        public string Bottomline { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
  
        public DateTime PostDate { get; set; }
        public Boolean Manset { get; set; }
        public Boolean HasGallery { get; set; }

        public virtual Gallery Gallery { get; set; }
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory{ get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

    }
}