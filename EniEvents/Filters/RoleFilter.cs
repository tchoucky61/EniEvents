using Bo;
using EniEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EniEvents.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserRoleAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var routeData = filterContext.HttpContext.Request.RequestContext.RouteData;
            var area = routeData.DataTokens["area"];

            if (area != null && (user == null || !user.IsInRole(UserRoles.ROLE_ADMIN)) && area.Equals("Admin"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "area", "Public" }, { "controller", "Home" }, { "action", "Index" } });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}