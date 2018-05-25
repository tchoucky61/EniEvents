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
using EniEvents.Areas.Admin.Models;
using System.Diagnostics;
using PagedList;

namespace EniEvents.Areas.Admin.Controllers
{
    public class ThemaController : Controller
    {
        
        private IRepository<Thema> repoThema;

        Context dbContext = new Context();
        public ThemaController()
        {
            repoThema = new GenericRepository<Thema>(this.dbContext);
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var themas = from t in dbContext.Themas
                         select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                themas = themas.Where(t => t.Title.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "title_desc":
                    themas = themas.OrderByDescending(t => t.Title);
                    break;
                default:
                    themas = themas.OrderBy(e => e.Title);
                    break;

            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(themas.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            var vm = new CreateEditThemaVM();
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEditThemaVM vm)
        {
            try
            {
                repoThema.Insert(vm.Thema);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return View();
            }            
        }

        public ActionResult Edit(int id)
        {
            var vm = new CreateEditThemaVM();
            vm.Thema = repoThema.GetById(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateEditThemaVM vm)
        {
            try
            {
                repoThema.Update(vm.Thema);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View(repoThema.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Thema th)
        {
            try
            {
                repoThema.Delete(th.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
