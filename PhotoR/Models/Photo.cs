using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Uri Uri { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}