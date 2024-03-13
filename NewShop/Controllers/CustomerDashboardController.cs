using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Controllers;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using NewShop.Models;

namespace NewShop.Controllers
{
    public class CustomerDashboardController : Controller
    {
        //
        // GET: /SrcSaleCoCrmStatus/ to CheckStatus / CustomerDashboard

        public ActionResult Index()
        {
            //this.Session["UserType"] = "";
            if (Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }
        public ActionResult Promotion()
        {

            if (Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();

        }
        public ActionResult PendingDeliver()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }

    }
}
