using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class Matter
    {
        public int Id { get; set; }
        public int GalleryId { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }


        public virtual Gallery Gallery { get; set; }
    }
}