using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhotoR.Models
{
    public class PhotoRDBContext : DbContext
    {
        public PhotoRDBContext() : base("DefaultConnection") { }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}