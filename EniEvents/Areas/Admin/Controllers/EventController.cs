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
using System.IO;
using PagedList;

namespace EniEvents.Areas.Admin.Controllers
{
    public class EventController : Controller
    {

        private EventRepository repoEvent;
        private IRepository<Thema> repoThema;

        Context Context = new Context();

        public EventController()
        {
            repoEvent = new EventRepository(Context);
            repoThema = new GenericRepository<Thema>(Context);
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.ThemaSortParm = String.IsNullOrEmpty(sortOrder) ? "thema_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var events = from e in Context.Events
                         select e;
            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Title.Contains(searchString)
                                       || e.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    events = events.OrderByDescending(e => e.Title);
                    break;
                case "Date":
                    events = events.OrderBy(e => e.Date);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(e => e.Date);
                    break;
                case "thema_desc":
                    events = events.OrderByDescending(e => e.Thema.Title);
                    break;
                default:
                    events = events.OrderBy(e => e.Title);
                    break;

            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(events.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            var vm = new CreateEditEventVM();
            vm.Themas = repoThema.GetAll();

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEditEventVM eventVM, HttpPostedFileBase[] upload)
        {
            try
            {
                if (eventVM.IdSelectedThema.HasValue)
                {
                    eventVM.Event.Thema = repoThema.GetById((int) eventVM.IdSelectedThema.Value);
                }

                List<Picture> pictures = new List<Picture>();
                foreach (HttpPostedFileBase file in upload)
                {
                    var extension = file.FileName.Split('.').Last<string>();
                    var InputFileName = Guid.NewGuid().ToString() + "." + extension;
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Content/media/img/") + InputFileName);
                    file.SaveAs(ServerSavePath);

                    var picture = new Picture();
                    picture.FileName = InputFileName;
                    pictures.Add(picture);
                }
                eventVM.Event.Pictures = pictures;

                repoEvent.Insert(eventVM.Event);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                eventVM.Themas = repoThema.GetAll();
                return View(eventVM);
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
        public ActionResult Edit(CreateEditEventVM vm, HttpPostedFileBase[] upload, string[] pictureIds)
        {
            try
            {
                Event ev = (Event)vm.Event.Clone();
                ev.Pictures = new List<Picture>();

                if (vm.IdSelectedThema.HasValue)
                {
                    ev.Thema = repoThema.GetById(vm.IdSelectedThema.Value);
                }

                // Delete pictures.
                var prevPictures = repoEvent.GetSet().Where(e => e.Id == vm.Event.Id).Include(e => e.Pictures).First().Pictures;

                foreach (Picture p in prevPictures)
                {
                    if (pictureIds.Contains(p.Id.ToString()))
                    {
                        ev.Pictures.Add(p);
                    }
                    else
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/media/img/") + p.FileName);
                    }
                }

                // Add new pictures.
                foreach (HttpPostedFileBase file in upload)
                {
                    if (file != null)
                    {
                        var extension = file.FileName.Split('.').Last<string>();
                        var InputFileName = Guid.NewGuid() + "." + extension;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Content/media/img/") + InputFileName);
                        file.SaveAs(ServerSavePath);

                        var picture = new Picture();
                        picture.FileName = InputFileName;
                        ev.Pictures.Add(picture);

                    }
                }

                repoEvent.Update(ev);
                return RedirectToAction("Index");
            }
            catch
            {
                vm.Themas = repoThema.GetAll();
                return View(vm);
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

                var pictures = from p in Context.Pictures select p;

                foreach (Picture p in pictures)
                {
                    if (p.Id == ev.Id)
                    {

                        Context.Pictures.Remove(p);

                    }
                }

                repoEvent.Delete(ev.Id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
