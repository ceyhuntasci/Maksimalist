using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        

        public virtual Category Category { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}