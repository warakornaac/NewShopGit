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
using NewShop.Attributes;

namespace NewShop.Controllers
{
    [Permission(
        "Index.GetSalesName",
        "Cart.Full"
        )]
    public class SeleScrCustomerController : Controller
    {
        public ActionResult CheckSession() {
            return Content(
                "IsNewSession = " + Session.IsNewSession +
                "<br/>SessionID = " + Session.SessionID +
                "<br/>UserType = " + (Session["UserType"] ?? "NULL")
            );
        }
        //
        // GET: /SeleScrCustomer/
        public ActionResult TestException() {
            this.Session["UserType"] = "";
            if (this.Session["UserType"] == null) {
                return RedirectToAction("LogIn", "Account");

            }


            //return View();
            throw new Exception("Test Application_Error");
        }

        public ActionResult Index()
        {
            //this.Session["UserType"] = "";
            if (this.Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");

            }
           
            return View();
        }
        public ActionResult dashboard(){
            //Response.Write("<br/>Session UserType dd = " + Session["UserType"]);
            //Response.End();
            this.Session["UserType"] = "";
            if (this.Session["UserType"] == null) {
                return RedirectToAction("LogIn", "Account");

            }


            return View();
            //throw new Exception("Test Application_Error");
        }
        public JsonResult Saveip(string ipno)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string messagereturn = string.Empty;
            SqlTransaction trans = null;
            try
            {
                SqlCommand cmd = new SqlCommand("p_SaveIP", Connection);
                cmd.Connection = Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IP", ipno);
               
                SqlParameter returnValue = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValue.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                cmd.ExecuteNonQuery();
                messagereturn = returnValue.Value.ToString();

            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                //return -1;
            }
            //return null;
            return Json(messagereturn, JsonRequestBehavior.AllowGet);
        }
    }
}
