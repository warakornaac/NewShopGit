using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Models;
using NewShop.Service;

namespace NewShop.Controllers
{
    public class ErrorController : Controller
    {
        //page show warning
        public ActionResult Index(int statusCode = 500, long id = 0) {
            ErrorViewModel model = new ErrorViewModel();

            model.StatusCode = statusCode;
            model.ButtonText = "Back to Dashboard";
            model.ButtonUrl = Url.Action("Login", "Account");
            switch (statusCode) {
                case 401:
                    model.Title = "Unauthorized";
                    model.Message = "Authentication Required";
                    model.Description = "Please login before using the system.";
                    model.PanelClass = "panel-danger";
                    model.Icon = "glyphicon-user";
                    break;
                case 403:
                    model.Title = "Access Denied";
                    model.Message = "You do not have permission to access this page.";
                    model.Description = "Your account does not have the required permission.";
                    model.PanelClass = "panel-warning";
                    model.Icon = "glyphicon-lock";
                    break;
                case 404:
                    model.Title = "Page Not Found";
                    model.Message = "The requested page could not be found.";
                    model.Description = "The page may have been moved or deleted.";
                    model.PanelClass = "panel-info";
                    model.Icon = "glyphicon-search";
                    break;
                default:
                    model.Title = "Internal Server Error";
                    model.Message = "An unexpected error occurred.";
                    model.Description = "Please contact the administrator if the problem persists.";
                    model.PanelClass = "panel-danger";
                    model.Icon = "glyphicon-warning-sign";
                    model.ErrorId = id;
                    if (id > 0) {
                        var error = ErrorLogService.GetError(id);
                        if (error != null) {
                            model.ErrorType = error.ErrorType;
                            model.StackTrace = error.StackTrace;
                        }
                    }
                    break;
            }
            model.IsAdmin = PermissionHelper.HasPermission("Admin.Full");
            Response.StatusCode = statusCode;
            return View("Error", model);
        }
        public ActionResult InternalServerError(long id) {
            var model = ErrorLogService.GetError(id);
            if (model == null) {
                return RedirectToAction("NotFound", "Error");
            }
            //System.Error.View
            model.IsAdmin = PermissionHelper.HasPermission("Admin.All");
            return View(model);
        }
    }
}
