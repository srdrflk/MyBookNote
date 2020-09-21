using MyBooknote.BusinessLayer;
using MyBooknote.Entities;
using MyBooknote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBooknote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CategoryManager cm = new CategoryManager();
        private BookNoteUserManager um = new BookNoteUserManager();

        // GET: Home
        public ActionResult Index()
        {
            var srdr = nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            return View(srdr);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = cm.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            BooknoteUser currentUser = Session["login"] as BooknoteUser;
            BusinessLayerResult<BooknoteUser> res = um.GetUserById(currentUser.Id);

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            BooknoteUser currentUser = Session["login"] as BooknoteUser;
            BusinessLayerResult<BooknoteUser> res = um.GetUserById(currentUser.Id);

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(BooknoteUser model)
        {
            BusinessLayerResult<BooknoteUser> res = um.UpdateProfile(model);

            if (ModelState.IsValid)
            {
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        public ActionResult DeleteProfile()
        {
            BooknoteUser currentUser = Session["login"] as BooknoteUser;
            BusinessLayerResult<BooknoteUser> res = um.RemoveUserById(currentUser.Id);

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<BooknoteUser> res = um.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                Session["login"] = res.Result;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<BooknoteUser> res = um.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");
            }
            return View(model);

        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<BooknoteUser> res = um.AktivateUser(id);

            return RedirectToAction("UserActivateOk");
        }

        public ActionResult UserActivateOk()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}