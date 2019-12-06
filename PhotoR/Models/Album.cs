using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}