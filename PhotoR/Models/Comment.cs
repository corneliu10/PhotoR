using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Content { get; set; }
    }
}