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
    public class EventController : Controller
    {

        private IRepository<Event> repoEvent;
        private IRepository<Thema> repoThema;

        Context dbContext = new Context();
        public EventController()
        {
            repoEvent = new EventRepository(this.dbContext);
            repoThema = new GenericRepository<Thema>(this.dbContext);
        }

        public ActionResult Index()
        {
            return View(repoEvent.GetAll());
        }

        public ActionResult Create()
        {
            var vm = new CreateEditEventVM();
            vm.Themas = repoThema.GetAll();
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEditEventVM eventVM)
        {
            try
            {
                if (eventVM.IdSelectedThema.HasValue)
                    eventVM.Event.Thema = repoThema.GetById(eventVM.IdSelectedThema.Value);

                repoEvent.Insert(eventVM.Event);
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
            var vm = new CreateEditEventVM();
            vm.Event = repoEvent.GetById(id);
            if (vm.Event.Thema != null)
            {
                vm.IdSelectedThema = vm.Event.Thema.Id;
            }

            vm.Themas = repoThema.GetAll();

            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateEditEventVM vm)
        {
            try
            {
                if (vm.IdSelectedThema.HasValue)
                {
                    vm.Event.Thema = repoThema.GetById(vm.IdSelectedThema.Value);
                }
                repoEvent.Update(vm.Event);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View(repoEvent.GetById(id));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Event ev)
        {
            try
            {
                repoEvent.Delete(ev.Id);
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
