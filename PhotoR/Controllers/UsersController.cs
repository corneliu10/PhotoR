﻿using PhotoR.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoR.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();
            var users = from user in db.Users
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

            var roleName = roles.FirstOrDefault(r => 
               r.Id == user.Roles.FirstOrDefault().RoleId).Name;
            var currentUserRole = roles.FirstOrDefault(j => j.Id ==
               currentUser.Roles.FirstOrDefault().RoleId).Name;

            var albums = db.Albums.Where(a => a.UserId == id);

            ViewBag.roleName = roleName;
            ViewBag.isAdmin = currentUserRole == "Administrator";
            ViewBag.albums = albums;

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
            // Commit pe articles
            db.SaveChanges();
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

    }
}