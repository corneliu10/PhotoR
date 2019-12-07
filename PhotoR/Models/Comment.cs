using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }
    }
}