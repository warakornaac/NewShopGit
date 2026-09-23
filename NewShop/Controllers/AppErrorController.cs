using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Models;
using NewShop.Service;

namespace NewShop.Controllers
{
    public class AppErrorController : Controller
    {
        public ActionResult Test() {
            return Content("OK");
        }
        //page show warning
        public ActionResult Index(int statusCode = 500, long id = 0) {
            ErrorViewModel model = new ErrorViewModel();

            model.StatusCode = statusCode;
            model.ButtonText = "Back to Dashboard";
            model.ButtonUrl = Url.Action("Login", "Account");
            switch (statusCode) {
                case 401:
                case 403:
                case 404:
                    model.Title = "Unable to Process Request";
                    model.Message = "The requested operation could not be completed.";
                    model.Description = "The system is currently unable to process your request. Please contact the system administrator if the problem persists.";
                    model.PanelClass = "panel-warning";
                    model.Icon = "glyphicon-warning-sign";
                    break;

                default:
                    model.Title = "System Unavailable";
                    model.Message = "The requested operation could not be completed.";
                    model.Description = "The system is currently experiencing a temporary issue. Please try again later or contact the system administrator if the problem continues.";
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
            //if (statusCode == 401 || statusCode == 403) {
                Response.StatusCode = 200;
            //}
            //else {
            //    Response.StatusCode = statusCode;
            //}

            Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = statusCode;
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
