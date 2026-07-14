using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NewShop.Service;

namespace NewShop.Attributes
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissions;

        public PermissionAttribute(params string[] permissions) {
            _permissions = permissions;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            // ==========================
            // 1. Session หมด
            // ==========================

            if (httpContext.Session == null) { 
                return false;
            }

            if (httpContext.Session["UserID"] == null) { 
                return false;
            }

            if (httpContext.Session["Permissions"] == null) { 
                return false;
            }

            List<string> permissions = httpContext.Session["Permissions"] as List<string>;

            if (permissions == null) { 
                return false;
            }
            //กรณีเป็น admin
            if (permissions.Contains("Admin.All")) {
                return true;
            }

            // ==========================
            // 2. เช็ค Permission
            // ==========================

            return _permissions.Any(
                 required =>
                     permissions.Contains(
                         required,
                         StringComparer.OrdinalIgnoreCase
                     )
             );
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            HttpContextBase context = filterContext.HttpContext;
            // Session หมด
            if (context.Session == null ||
                context.Session["UserID"] == null) {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                        { "controller","Account" },
                        { "action","Login" }
                        });
                return;
            }
            // ไม่มีสิทธิ์เข้าบันทึกลง PermissionDeniedLogs
            //PermissionLogService.LogDenied(filterContext);
            PermissionLogService.LogDenied(context, string.Join(",", _permissions));

            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller","Error" },
                    { "action","Index" },
                    { "statusCode",403 }
                    });
        }
    }
}