using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PhotoR.Models;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(PhotoR.Startup))]
namespace PhotoR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com"
                };
                var adminCreated = userManager.Create(user, "Admin1!");
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                    context.Albums.Add(new Album
                    {
                        Name = "Default",
                        CreatedAt = System.DateTime.Now,
                        UserId = user.Id,
                    });
                    context.SaveChanges();
                }
            }

            if (context.Categories.Count() == 0)
            {
                context.Categories.AddRange(new List<Category>{
                    new Category
                    {
                        Name = "Nature",
                        CreatedAt = System.DateTime.Now,
                    },
                    new Category
                    {
                        Name = "Art",
                        CreatedAt = System.DateTime.Now
                    }
                });
                context.SaveChanges();
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };
                roleManager.Create(role);
            }
            context?.Dispose();
            roleManager?.Dispose();
            userManager?.Dispose();
        }
    }
}
