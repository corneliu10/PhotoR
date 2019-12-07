using PhotoR.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoR.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();
            var users = from user in db.Users
                        where user.Id != currentUser
                        orderby user.UserName
                        select user;
            ViewBag.UsersList = users;
            return View();
        }

        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var currentUserId = User.Identity.GetUserId();
            ViewBag.currentUser = currentUserId;
            ApplicationUser currentUser = db.Users.Find(currentUserId);
            
            var roles = db.Roles.ToList();

            var roleName = roles.Where(j => j.Id ==
               user.Roles.FirstOrDefault().RoleId).
               Select(a => a.Name).FirstOrDefault();
            var currentUserRole = roles.Where(j => j.Id ==
               currentUser.Roles.FirstOrDefault().RoleId).
               Select(a => a.Name).FirstOrDefault();

            ViewBag.roleName = roleName;
            ViewBag.isAdmin = currentUserRole == "Administrator";

            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {

            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            //sterge si toate pozele, albumele, comentariile user ului
            //var articles = db.Articles.Where(a => a.UserId == id);
            //foreach (var article in articles)
            //{
            //    db.Articles.Remove(article);

            //}
            // Commit pe articles
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

    }
}