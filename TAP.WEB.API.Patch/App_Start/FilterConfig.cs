using System.Web;
using System.Web.Mvc;

namespace TAP.WEB.API.Patch
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
