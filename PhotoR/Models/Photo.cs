using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoR.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}