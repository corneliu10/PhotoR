using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Album
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}