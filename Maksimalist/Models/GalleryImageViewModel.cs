using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class GalleryImageViewModel
    {
        public string ImageContent { get; set; }
        public Gallery Gallery { get; set; }
        public string Url { get; set; }

        
    }
    public class GalleryImageListViewModel
    {
        public IEnumerable<GalleryImageViewModel> GalleryImageViewModel { get; set; }
    }
}