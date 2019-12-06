using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Uri { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
        
        public virtual IdentityUser User { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}