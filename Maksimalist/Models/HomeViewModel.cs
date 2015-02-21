using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class HomeViewModel
    {
        public ICollection<Post> Manset { get; set; }
        public ICollection<Post> Moda { get; set; }
        public ICollection<Post> Guzellik { get; set; }
        public ICollection<Post> Alisveris { get; set; }
        public ICollection<Post> Unluler { get; set; }
        public ICollection<Post> SehirYasam { get; set; }
        public ICollection<Post> Gelin { get; set; }
        public ICollection<Post> Video { get; set; }
  
    }
}