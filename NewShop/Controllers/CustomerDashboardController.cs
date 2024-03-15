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
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=amount");
            }
            return View();
        }
        public ActionResult Promotion()
        {

            if (Session["UserType"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=promotion");

            }
            return View();

        }
        public ActionResult PendingDeliver()
        {
            if (Session["UserType"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=PendingDeliver");

            }
            return View();
        }
        public ActionResult CustomerAlert()
        {
            return View();
        }

    }
}
