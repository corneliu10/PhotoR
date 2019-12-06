using Microsoft.AspNet.Identity;
using PhotoR.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhotoR.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var photos = from photo in db.Photos
                         select photo;
            ViewBag.Photos = photos;
            return View();
        }

        public ActionResult Show(int id)
        {
            Photo photo = db.Photos.Find(id);
            return View(photo);
        }

        [Authorize(Roles = "User,Administrator")]
        public ActionResult New()
        {
            var categories = from category in db.Categories
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult New(Photo photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    photo.CreatedAt = DateTime.Now;
                    photo.UserId = User.Identity.GetUserId();
                    db.Photos.Add(photo);
                    db.SaveChanges();
                    TempData["message"] = "Photo was saved!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(photo);
                }
            }
            catch (Exception e)
            {
                return View(photo);
            }
        }

        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id)
        {
            Photo photo = db.Photos.Find(id);
            return View(photo);
        }

        [HttpPut]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id, Photo requestPhoto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Photo photo = db.Photos.Find(id);
                    if (TryUpdateModel(photo))
                    {
                        photo.Uri = requestPhoto.Uri;
                        TempData["message"] = "Photo was changed!";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestPhoto);
                }
            }
            catch (Exception e)
            {
                return View(requestPhoto);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Delete(int id)
        {
            Photo photo = db.Photos.Find(id);

            db.Photos.Remove(photo);
            TempData["message"] = "Photo was deleted!";
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}