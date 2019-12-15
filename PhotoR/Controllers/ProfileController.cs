using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoR.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(currentUserId);

            var albums = db.Albums.Where(a => a.UserId == currentUserId);
            ViewBag.albums = albums;

            return View(user);
        }

        public ActionResult Delete()
        {
            var id = User.Identity.GetUserId();
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            //sterge si toate pozele, albumele, comentariile user ului
            var albums = db.Albums.Where(a => a.UserId == id);
            foreach (var album in albums)
            {
                db.Albums.Remove(album);
            }
            var photos = db.Photos.Where(a => a.UserId == id);
            foreach (var photo in photos)
            {
                db.Photos.Remove(photo);
            }
            var comments = db.Comments.Where(a => a.UserId == id);
            foreach (var comment in comments)
            {
                db.Comments.Remove(comment);
            }
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Login", "Account");
        }
    }
}