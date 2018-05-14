using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bo;
using Dal;

namespace EniEvents.Controllers
{
    public class ThemaController : Controller
    {
        private Context db = new Context();

        // GET: Thema
        public ActionResult Index()
        {
            return View(db.Themas.ToList());
        }

        // GET: Thema/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // GET: Thema/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thema/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Thema thema)
        {
            if (ModelState.IsValid)
            {
                db.Themas.Add(thema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thema);
        }

        // GET: Thema/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // POST: Thema/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Thema thema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thema);
        }

        // GET: Thema/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // POST: Thema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thema thema = db.Themas.Find(id);
            db.Themas.Remove(thema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
