using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class MobileHomeViewModel
    {
        public Post Manset { get; set; }
        public ICollection<Post> Postlar { get; set; }
    }
}