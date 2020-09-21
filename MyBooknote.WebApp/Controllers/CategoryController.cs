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
    public class CategoryController : Controller
    {
        private CategoryManager categorymanager = new CategoryManager();

        public ActionResult Index()
        {
            return View(categorymanager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                categorymanager.Insert(category);
                categorymanager.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Category cat = categorymanager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;

                categorymanager.Update(cat);
                //categorymanager.Save();

                return RedirectToAction("Index");
            }
            return View(category);
            
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // ilişkili tabloyu silmek için SQL Server da Diagramda ilişkili tabloların ilişki propertiesinde INSERT and UPDATE specification seçeneğini CASCADE olarak değiştirmek yeterli.

            Category category = categorymanager.Find(x => x.Id == id);
            categorymanager.Delete(category);
            categorymanager.Save();
            return RedirectToAction("Index");
        }
    }
}
