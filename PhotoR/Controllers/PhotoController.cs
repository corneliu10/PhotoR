using Microsoft.AspNet.Identity;
using PhotoR.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using PhotoR.Helpers;
using System.Web;
using System.IO;

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

        public ActionResult Search(string description)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var photos = from photo in db.Photos
                         where photo.Description.StartsWith(description)
                         select photo;
            ViewBag.Photos = photos;
            return View("Index");
        }

        public ActionResult Show(int id)
        {
            Photo photo = db.Photos.Find(id);

            // get Comments
            ViewBag.Comments = db.Comments.Where(c => c.PhotoId == id).OrderByDescending(c => c.CreatedAt);
            ViewBag.CurrentUser = UsersHelper.GetUserById(User.Identity.GetUserId());
            ViewBag.IsAdmin = UsersHelper.IsUserAdmin(ViewBag.CurrentUser);

            return View(photo);
        }

        [Authorize(Roles = "User,Administrator")]
        public ActionResult New()
        {
            var categories = from category in db.Categories
                             select category;
            var userId = User.Identity.GetUserId();
            var albums = db.Albums.Where(a => a.UserId == userId);
            ViewBag.Albums = albums.ToList();
            ViewBag.Categories = categories.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult New(string description, int categoryId, int albumId, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = null;
                    if (file != null)
                    {
                        if (file.ContentLength <= 0)
                        {
                            throw new Exception("Error while uploading");
                        }

                        fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Assets"), fileName);
                        file.SaveAs(path);
                    }

                    var photo = new Photo
                    {
                        Description = description,
                        CategoryId = categoryId,
                        FileName = fileName
                    };
                    photo.CreatedAt = DateTime.Now;
                    photo.UserId = User.Identity.GetUserId();
                    photo.AlbumId = albumId;
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

        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id)
        {
            Photo photo = db.Photos.Find(id);
            var categories = from category in db.Categories
                             select category;
            var userId = User.Identity.GetUserId();
            var albums = db.Albums.Where(a => a.UserId == userId);
            ViewBag.Categories = categories;
            ViewBag.Albums = albums;
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
                        photo.FileName = requestPhoto.FileName;
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

        [HttpDelete]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult DeleteComment(int commentId, int photoId)
        {
            Comment comment = db.Comments.Find(commentId);

            TempData["message"] = $"Comment '{comment.Content}' was deleted!";
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction($"Show/{photoId}");
        }

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult AddComment(string content, int photoId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = new Comment
                    {
                        Content = content,
                        UserId = User.Identity.GetUserId(),
                        PhotoId = photoId,
                        CreatedAt = DateTime.Now
                    };
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Comment was saved!";
                    return RedirectToAction("Show/" + photoId);
                }
                else
                {
                    return RedirectToAction("Show/" + photoId);
                }
            }
            catch (Exception e)
            {
                //return View(photo);
            }

            return null;
        }

        [HttpPut]
        [Authorize(Roles = "User,Administrator")]
        public ActionResult EditComment(int id, Comment requestComment, int photoId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = db.Comments.Find(id);
                    if (TryUpdateModel(comment))
                    {
                        comment.Content = requestComment.Content;
                        TempData["message"] = "Comment was changed!";
                        db.SaveChanges();
                    }
                    return RedirectToAction($"Show/{photoId}");
                }
                else
                {
                    return View($"Show/{photoId}");
                }
            }
            catch (Exception e)
            {
                return View($"Show/{photoId}");
            }
        }

        [Authorize(Roles = "User,Administrator")]
        public ActionResult ImageEditor(int id)
        {
            var photo = db.Photos.Find(id);
            if (photo != null)
            {
                ViewBag.fileName = photo.FileName;
            }
            return View();
        }
    }
}