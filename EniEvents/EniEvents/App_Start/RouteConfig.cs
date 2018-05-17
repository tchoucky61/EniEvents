using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EniEvents
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "AdminEventEdit",
               url: "admin/event/edit/{id}",
               defaults: new { controller = "Event", action = "Edit", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminEventDelete",
               url: "admin/event/delete/{id}",
               defaults: new { controller = "Event", action = "Delete", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminEventNew",
               url: "admin/event/new",
               defaults: new { controller = "Event", action = "Create", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminEventList",
               url: "admin/events",
               defaults: new { controller = "Event", action = "Index" }
           );

            routes.MapRoute(
               name: "AdminThemaEdit",
               url: "admin/thema/edit/{id}",
               defaults: new { controller = "Thema", action = "Edit", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminThemaDelete",
               url: "admin/thema/delete/{id}",
               defaults: new { controller = "Thema", action = "Delete", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminThemaNew",
               url: "admin/thema/new",
               defaults: new { controller = "Thema", action = "Create", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AdminThemaList",
               url: "admin/themas",
               defaults: new { controller = "Thema", action = "Index" }
           );

            routes.MapRoute(
                 name: "EventJson",
                 url: "event/get/{period}/{themaId}",
                 defaults: new { controller = "Home", action = "EventJson", period = UrlParameter.Optional, themaId = UrlParameter.Optional }
             );

            routes.MapRoute(
               name: "EventDetails",
               url: "event/details/{id}",
               defaults: new { controller = "Home", action = "EventDetails", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
