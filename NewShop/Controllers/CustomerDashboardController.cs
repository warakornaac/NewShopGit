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
using System.DirectoryServices.Protocols;

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
        public ActionResult DeliveryTrack()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }
        public ActionResult CustomerAlert()
        {
            return View();
        }

        public JsonResult Credit_Cus(string cuscod)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CustomerCredit> credits = new List<CustomerCredit>();
            string message = "";
            try
            {
                var cmd = new SqlCommand("P_Customer_credit", Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CUSCOD", cuscod);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    credits.Add(new CustomerCredit()
                    {
                        CUSCOD = reader["CUSCOD"].ToString(),
                        CUSNAME = reader["CUSNAM"].ToString(),
                        ADDR_01 = reader["ADDR_01"].ToString(),
                        ADDR_02 = reader["ADDR_02"].ToString(),
                        PRO = reader["PRO"].ToString(),
                        CUSTYP = reader["CUSTYP"].ToString(),
                        AACCRLINE = reader["AACCRLINE"].ToString(),
                        AACBAL = reader["AACBAL"].ToString(),
                        AACBALDue = reader["AACBALDue"].ToString(),
                        TACCRLINE = reader["TACCRLINE"].ToString(),
                        TACBAL = reader["TACBAL"].ToString(),
                        TACBALDue = reader["TACBALDue"].ToString(),
                        OMPCRLINE = reader["OMPCRLINE"].ToString(),
                        OMPBAL = reader["OMPBAL"].ToString(),
                        OMPBALDue = reader["OMPBALDue"].ToString(),
                        SLMCOD = reader["SLMCOD"].ToString(),
                        INACTIVE = reader["INACTIVE"].ToString(),
                        BLOCKED = reader["BLOCKED"].ToString(),
                        AACPAYTR = reader["AACPAYTRM"].ToString(),
                        TACPAYTR = reader["TACPAYTRM"].ToString(),
                        OMPPAYTR = reader["OMPPAYTRM"].ToString(),
                        TELNUM = reader["TELNUM"].ToString()
                    });
                }
                reader.Close();
                cmd.Dispose();
                Connection.Close();
                message = "Y";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, credits }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getbackorder_notify(string CUSCOD)
        {
            string message = "";
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<BackOrder_Notify> backorder = new List<BackOrder_Notify>();
            try
            {
                var command = new SqlCommand("P_Search_BackOrder_Notify", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD);
                SqlDataReader drb = command.ExecuteReader();
                while (drb.Read())
                {
                    backorder.Add(new BackOrder_Notify()
                    {
                        STKCOD = drb["STKCOD"].ToString(),
                        STKDES = drb["STKDES"].ToString(),
                        Qty = drb["Qty"].ToString(),
                        SaleOrderDate = Convert.ToDateTime(drb["SaleOrder_Date"]).ToString("dd/MM/yyyy"),
                        DeliveryDate = drb["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(drb["DeliveryDate"]).ToString("dd/MM/yyyy") : ""
                    });
                }
                drb.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message = message, backorder }, JsonRequestBehavior.AllowGet);
        }

    }
}
