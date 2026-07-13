using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using NewShop.Service;

namespace NewShop
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error() {
            Exception ex = Server.GetLastError();

            if (ex == null)
                return;
            var httpEx = ex as HttpException;

            if (httpEx != null) {
                int code = httpEx.GetHttpCode();

                // ไม่ต้อง Log 404
                if (code == 404) {
                    Server.ClearError();
                    return;
                }
            }

            // ไม่ Log ถ้าเป็น ErrorController เอง
            var route = RouteTable.Routes.GetRouteData(
                new HttpContextWrapper(Context));

            string controller = route?.Values["controller"]?.ToString();

            if (string.Equals(controller, "Error",
                StringComparison.OrdinalIgnoreCase)) {
                Server.ClearError();
                return;
            }

            long errorId = ErrorLogService.Log(ex);

            Server.ClearError();

            Response.Redirect(
                "~/Error/Index?statusCode=500&id=" + errorId,
                false);

            Context.ApplicationInstance.CompleteRequest();
        }
    }
}