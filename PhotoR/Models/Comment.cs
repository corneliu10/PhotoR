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

        public int UserId { get; set; }

        public string Content { get; set; }
    }
}