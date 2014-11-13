using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class Gallery
    {
        public int Id { get; set; }
      
        public string Name { get; set; }
        public virtual ICollection<Matter> Matter { get; set; }
        
    }
}