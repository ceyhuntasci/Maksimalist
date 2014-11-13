using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Maksimalist.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Matter> Matter { get; set; }

        public virtual Post Post { get; set; }
        
    }
}