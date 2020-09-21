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
using MyBooknote.WebApp.Models;

namespace MyBooknote.WebApp.Controllers
{
    public class NotesController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CategoryManager cm = new CategoryManager();
        private LikedManager likedM = new LikedManager();
        // GET: Notes
        public ActionResult Index()
        {
            var notes = nm.ListQueryable().Include("Category").Include("Owner").Where(x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(x => x.ModifiedOn);

            return View(notes.ToList());
            //return View(nm.List());
        }

        public ActionResult MyLikedNotes()
        {
            var notes = likedM.ListQueryable().Include("LikedUser").Include("Note").Where(x => x.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note).Include("Category").Include("Owner").OrderByDescending(x => x.ModifiedOn);

            return View("Index",notes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                nm.Insert(note);
                nm.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
               Note db_not = nm.Find(x => x.Id == note.Id);
                db_not.Title = note.Title;
                db_not.Text = note.Text;
                db_not.CategoryId = note.CategoryId;
                db_not.IsDraft = note.IsDraft;
                nm.Update(db_not);
                nm.Save();

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(x => x.Id == id);
            nm.Delete(note);
            nm.Save();
            return RedirectToAction("Index");
        }

    }
}
