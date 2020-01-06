using PhotoR.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace PhotoR.Helpers
{
    public static class UsersHelper
    {
        private static readonly ApplicationDbContext db = new ApplicationDbContext();

        public static string GetUserRoleName(ApplicationUser user)
        {
            if (user == null)
            {
                return "NotUser";
            }

            var roles = db.Roles.ToList();

            return roles.FirstOrDefault(j => j.Id ==
               user.Roles.FirstOrDefault().RoleId).Name ?? "";
        }

        public static bool IsUserAdmin(ApplicationUser user)
        {
            var roleName = GetUserRoleName(user);

            return roleName.Equals("Administrator");
        }

        public static ApplicationUser GetUserById(string id)
        {
            var user = db.Users.Find(id);

            return user;
        }
    }
}