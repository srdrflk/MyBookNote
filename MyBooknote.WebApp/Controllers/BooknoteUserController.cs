using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBooknote.BusinessLayer;
using MyBooknote.Entities;

namespace MyBooknote.WebApp.Controllers
{
    public class BooknoteUserController : Controller
    {
        private BookNoteUserManager bum = new BookNoteUserManager();

        public ActionResult Index()
        {
            return View(bum.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooknoteUser booknoteUser = bum.Find(x => x.Id == id.Value);
            if (booknoteUser == null)
            {
                return HttpNotFound();
            }
            return View(booknoteUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooknoteUser booknoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<BooknoteUser> res = bum.Insert(booknoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(booknoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(booknoteUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooknoteUser booknoteUser = bum.Find(x => x.Id == id.Value);
            if (booknoteUser == null)
            {
                return HttpNotFound();
            }
            return View(booknoteUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BooknoteUser booknoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<BooknoteUser> res = bum.Update(booknoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x));
                    return View(booknoteUser);
                }

                return RedirectToAction("Index");
            }
            return View(booknoteUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooknoteUser booknoteUser = bum.Find(x => x.Id == id.Value);
            if (booknoteUser == null)
            {
                return HttpNotFound();
            }
            return View(booknoteUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BooknoteUser booknoteUser = bum.Find(x => x.Id == id);
            bum.Delete(booknoteUser);

            return RedirectToAction("Index");
        }
    }
}
