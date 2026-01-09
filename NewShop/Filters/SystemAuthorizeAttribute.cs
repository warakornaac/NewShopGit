using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewShop.Filters 
{
    public class SystemAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null) { 
                return false;
            }
            // ดึงค่า UserID
            string userId = httpContext.Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId)) { 
                return false;
            }
            // เช็ค system
            string systemCode = ConfigurationManager.AppSettings["SystemCode"];
            string loginSystem = httpContext.Session["LoginSystem"]?.ToString();
            if (string.IsNullOrEmpty(systemCode)) { 
                return false;
            }

            return loginSystem == systemCode;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                })
            );
        }
    }
}
