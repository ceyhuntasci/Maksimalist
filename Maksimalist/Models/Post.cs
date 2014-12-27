using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        [DisplayName("Alt Kategori")]
        public int? SubCategoryId { get; set; }
        [DisplayName("Yazar")]
        public int AuthorId { get; set; }
        
        public int? GalleryId { get; set; }
        [DisplayName("Başlık")]
        public string Headline { get; set; }
        [DisplayName("Alt Başlık")]
        public string Bottomline { get; set; }
        [AllowHtml]
        [DisplayName("İçerik")]
        public string Content { get; set; }
        public string UrlSlug { get; set; }
        [DisplayName("Manşet Görseli")]
        public string ImageUrl { get; set; }
        [DisplayName("İçerik Görseli")]
        public string ContentImage { get; set; }
        [DisplayName("Video Linki")]
        [AllowHtml]
        public string VideoUrl { get; set; }
        [DisplayName("Tarih")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime PostDate { get; set; }

        public Boolean Manset { get; set; }
        [DisplayName("Galerisi Var Mı?")]
        public Boolean HasGallery { get; set; }
        [DisplayName("Video Haberi")]
        public Boolean HasVideo { get; set; }
        public int HitCount { get; set; }
        public virtual Gallery Gallery { get; set; }
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory{ get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

    }
}