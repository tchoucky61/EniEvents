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
                name: "Welcome",
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                },
               namespaces: new string[] { "EniEvents.Controllers" }
            );


            routes.MapRoute(
               name: "ParkList",
               url: "Public/Home/ParkList/{id}/{startAddress}",
               defaults: new { controller = "Home", action = "ParkList", id = UrlParameter.Optional, startAddress = UrlParameter.Optional },
               namespaces: new string[] { "EniEvents.Controllers" }
           );

            routes.MapRoute(
                 name: "EventJson",
                 url: "Public/Home/EventJson/{period}/{themaId}",
                 defaults: new { controller = "Home", action = "EventJson", period = UrlParameter.Optional, themaId = UrlParameter.Optional },
               namespaces: new string[] { "EniEvents.Controllers" }
             );

            routes.MapRoute(
               name: "EventDetails",
               url: "Public/Home/EventDetails/{id}",
               defaults: new { controller = "Home", action = "EventDetails", id = UrlParameter.Optional },
               namespaces: new string[] { "EniEvents.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                },
               namespaces: new string[] { "EniEvents.Controllers" }
            );

        }
    }
}
