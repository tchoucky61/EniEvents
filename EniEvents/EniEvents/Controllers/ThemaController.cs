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
using EniEvents.Models;
using System.Diagnostics;

namespace EniEvents.Controllers
{
    public class ThemaController : Controller
    {
        
        private IRepository<Thema> repoThema;

        Context dbContext = new Context();
        public ThemaController()
        {
            repoThema = new GenericRepository<Thema>(this.dbContext);
        }

        public ActionResult Index()
        {
            return View(repoThema.GetAll());
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
