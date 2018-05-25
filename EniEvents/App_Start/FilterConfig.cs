using Bo;
using EniEvents.Filters;
using System.Web;
using System.Web.Mvc;

namespace EniEvents
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CheckUserRoleAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
