using System.Web;
using System.Web.Mvc;
using NewShop.Filters;

namespace NewShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //ทุก request ต้องผ่าน SystemAuthorize
            //filters.Add(new SystemAuthorizeAttribute());
        }
    }
}