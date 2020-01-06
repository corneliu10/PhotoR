using Microsoft.AspNet.Identity;
using PhotoR.Helpers;
using PhotoR.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhotoR.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class AlbumController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var albums = from album in db.Albums
                         orderby album.Name
                         select album;
            ViewBag.Albums = albums;
            return View();
        }

        public ActionResult Show(int id)
        {
            Album album = db.Albums.Find(id);
            var allUserPhotos = from photo in db.Photos
                                select photo;
            ViewBag.AllUserPhotos = allUserPhotos;
            return View(album);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Album album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    album.CreatedAt = DateTime.Now;
                    album.UserId = User.Identity.GetUserId();
                    db.Albums.Add(album);
                    db.SaveChanges();
                    TempData["message"] = "Album was saved!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(album);
                }
            }
            catch (Exception e)
            {
                return View(album);
            }
        }

        public ActionResult Edit(int id)
        {
            Album album = db.Albums.Find(id);
            return View(album);
        }

        [HttpPut]
        public ActionResult Edit(int id, Album requestAlbum)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Album album = db.Albums.Find(id);
                    if (TryUpdateModel(album))
                    {
                        album.Name = requestAlbum.Name;
                        TempData["message"] = "Album was changed!";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestAlbum);
                }
            }
            catch (Exception e)
            {
                return View(requestAlbum);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Album album = db.Albums.Find(id);

            db.Albums.Remove(album);
            TempData["message"] = "Album was deleted!";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddPhoto(int albumId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var photo = new Photo
                    {
                       
                    };
                    photo.CreatedAt = DateTime.Now;
                    photo.UserId = User.Identity.GetUserId();
                    db.Photos.Add(photo);
                    db.SaveChanges();
                    TempData["message"] = "Photo was saved!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("New");
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}