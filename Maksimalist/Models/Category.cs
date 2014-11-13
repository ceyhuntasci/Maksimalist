using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}