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
using System.Web.Services.Description;

namespace NewShop.Controllers
{
    public class CustomerDashboardController : Controller
    {
        //
        // GET: /SrcSaleCoCrmStatus/ to CheckStatus / CustomerDashboard

        public ActionResult Index()
        {
            //this.Session["UserType"] = "";
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=amount");
            }
            return View();
        }
        public ActionResult CustomerMenu()
        {
            //this.Session["UserType"] = "";
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=menu");
            }
            return View();
        }
        public ActionResult Promotion()
        {

            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=promotion");
            }
            return View();

        }
        public ActionResult PendingDeliver()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=PendingDeliver");
            }
            return View();
        }
        public ActionResult PendingDeliver_Bk()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=PendingDeliver");
            }
            return View();
        }
        public ActionResult DeliveryTrack()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=DeliveryTrack");
            }
            return View();
        }
        public ActionResult Warranty()
        {

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
                        LASIVC = reader["LASIVC"] != DBNull.Value ? Convert.ToDateTime(reader["LASIVC"]).ToString("dd/MM/yyyy") : "",
                        AACCRLINE = reader["AACCRLINE"].ToString(),
                        AACBAL = reader["AACBAL"].ToString(),
                        AACBALDue = reader["AACBALDue"].ToString(),
                        AACBilDue = reader["AACBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["AACBilDue"]).ToString("dd/MM/yyyy") : "",
                        TACCRLINE = reader["TACCRLINE"].ToString(),
                        TACBAL = reader["TACBAL"].ToString(),
                        TACBALDue = reader["TACBALDue"].ToString(),
                        TACBilDue = reader["TACBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["TACBilDue"]).ToString("dd/MM/yyyy") : "",
                        OMPCRLINE = reader["OMPCRLINE"].ToString(),
                        OMPBAL = reader["OMPBAL"].ToString(),
                        OMPBALDue = reader["OMPBALDue"].ToString(),
                        OMPBilDue = reader["OMPBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["OMPBilDue"]).ToString("dd/MM/yyyy") : "",
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
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    backorder.Add(new BackOrder_Notify()
                    {
                        Company = reader["Company"].ToString(),
                        Document_No = reader["Document No"].ToString(),
                        SONUM = reader["SONUM"].ToString(),
                        STKCOD = reader["STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        Qty = reader["Qty"].ToString(),
                        SalePrice = Convert.ToDecimal(reader["SalePrice"]).ToString("F2"),
                        Amount = Convert.ToDecimal(reader["Amount"]).ToString("F2"),
                        SaleOrderDate = Convert.ToDateTime(reader["SaleOrder_Date"]).ToString("dd/MM/yyyy"),
                        DeliveryDate = reader["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["DeliveryDate"]).ToString("dd/MM/yyyy") : ""
                    });
                }
                reader.Close();
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

        public JsonResult GetDeliveryTracking(string CUSCOD)
        {
            string message = "";
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<DeliveryTrackNotify> Getdata = new List<DeliveryTrackNotify>();
            try
            {
                var command = new SqlCommand("p_Order_Notify_List_Test", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new DeliveryTrackNotify()
                    {
                        ORD_DocNo = reader["ORD_DocNo"].ToString(),
                        CUSCOD = reader["CUSCOD"].ToString(),
                        NotifyID = reader["NotifyID"].ToString(),
                        Notify = reader["Notify"].ToString(),
                        StatusDate = Convert.ToDateTime(reader["StatusDate"]).ToString("dd/MM/yyyy"),
                        Order_Date = Convert.ToDateTime(reader["Order Date"]).ToString("dd/MM/yyyy"),
                        ORD_TotalItem = reader["ORD_TotalItem"].ToString(),
                        ORD_TotalQty = reader["ORD_TotalQty"].ToString(),
                        ORD_TotalAmt = reader["ORD_TotalAmt"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeliveryTrackTab(string CUSCOD, string NotifID)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<DeliveryTrackNotify> Getdata = new List<DeliveryTrackNotify>();
            try
            {
                var command = new SqlCommand("p_Order_Notify_List_Test", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@NotifyID", NotifID);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new DeliveryTrackNotify()
                    {
                        ORD_DocNo = reader["ORD_DocNo"].ToString(),
                        CUSCOD = reader["CUSCOD"].ToString(),
                        NotifyID = reader["NotifyID"].ToString(),
                        Notify = reader["Notify"].ToString(),
                        StatusDate = Convert.ToDateTime(reader["StatusDate"]).ToString("dd/MM/yyyy"),
                        Order_Date = Convert.ToDateTime(reader["Order Date"]).ToString("dd/MM/yyyy"),
                        ORD_TotalItem = reader["ORD_TotalItem"].ToString(),
                        ORD_TotalQty = reader["ORD_TotalQty"].ToString(),
                        ORD_TotalAmt = reader["ORD_TotalAmt"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {

            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSuccessfulDeliveryByMonth(string CUSCOD, string MONTH)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<DeliveryTrackNotify> Getdata = new List<DeliveryTrackNotify>();
            try
            {
                var command = new SqlCommand("p_Order_Notify_List_Test", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@NotifyID", "4");
                command.Parameters.AddWithValue("Month", MONTH);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new DeliveryTrackNotify()
                    {
                        ORD_DocNo = reader["ORD_DocNo"].ToString(),
                        CUSCOD = reader["CUSCOD"].ToString(),
                        NotifyID = reader["NotifyID"].ToString(),
                        Notify = reader["Notify"].ToString(),
                        StatusDate = Convert.ToDateTime(reader["StatusDate"]).ToString("dd/MM/yyyy"),
                        Order_Date = Convert.ToDateTime(reader["Order Date"]).ToString("dd/MM/yyyy"),
                        ORD_TotalItem = reader["ORD_TotalItem"].ToString(),
                        ORD_TotalQty = reader["ORD_TotalQty"].ToString(),
                        ORD_TotalAmt = reader["ORD_TotalAmt"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {

            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeliveryDetail(string ORD_DocNo)
        {
            string message = "";
            List<SaleOrderDetail> Getdata = new List<SaleOrderDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Order_Notify_List_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inORD_DocNo", ORD_DocNo);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new SaleOrderDetail()
                    {
                        RowNo = reader["RowNo"].ToString(),
                        VSTKCOD = reader["ORD_STKCOD"].ToString(),
                        VSTKDES = reader["STKDES"].ToString(),
                        VSTKGRP = reader["ORD_STKGRP"].ToString(),
                        VPrice = reader["ORD_Price"].ToString(),
                        VSalePrice = reader["ORD_SalePrice"].ToString(),
                        VORDDAT = reader["ORD_Date"].ToString(),
                        Item_Type = reader["Item_Type"].ToString(),
                        VDiscount = reader["ORD_Discount"].ToString(),
                        AmtQty = reader["ORD_Qty"].ToString(),
                        TotalAmt = reader["ORD_Amt"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCusMonth(string CUSCOD, string Month, string Year)
        {
            string message = "";
            List<CusAmtMonth> Getdata = new List<CusAmtMonth>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Search_Cus_Month", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD.Trim());
                command.Parameters.AddWithValue("@inYear", Year.Trim());
                command.Parameters.AddWithValue("@inMonth", Month.Trim());
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new CusAmtMonth()
                    {
                        Peiord = reader["Peiord"].ToString(),
                        CUSNAM = reader["CUSNAM"].ToString(),
                        Amount = String.Format("{0:N2}", Convert.ToDecimal(reader["Amount"]))
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex) { message = ex.Message; }

            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCusInvMonth(string CUSCOD, string Month, string Year)
        {
            string message = "";
            List<Cusinv_Month> Getdata = new List<Cusinv_Month>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Search_Cusinv_Month", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD.Trim());
                command.Parameters.AddWithValue("@inYear", Year.Trim());
                command.Parameters.AddWithValue("@inMonth", Month.Trim());
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Cusinv_Month()
                    {
                        Company = reader["Company"].ToString(),
                        PSTDAT = Convert.ToDateTime(reader["PSTDAT"]).ToString("dd/MM/yyyy"),
                        DUEDAT = Convert.ToDateTime(reader["DUEDAT"]).ToString("dd/MM/yyyy"),
                        EXTDOC = reader["EXTDOC"].ToString(),
                        CUSNAM = reader["CUSNAM"].ToString(),
                        DOCNUM = reader["DOCNUM"].ToString(),
                        Amount = reader["Amount"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex) { message = ex.Message; }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCusinvDetail(string Docno)
        {
            string message = string.Empty;
            List<Cusinv_Item> Getdata = new List<Cusinv_Item>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Search_Cusinv_Month_Item", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inDocno", Docno);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Cusinv_Item()
                    {
                        Company = reader["Company"].ToString(),
                        PSTDAT = reader["PSTDAT"].ToString(),
                        DUEDAT = reader["DUEDAT"].ToString(),
                        EXTDOC = reader["EXTDOC"].ToString(),
                        CUSNAM = reader["CUSNAM"].ToString(),
                        DOCNUM = reader["DOCNUM"].ToString(),
                        STKCOD = reader["STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        Qty = reader["Qty"].ToString(),
                        Unit_Price = reader["Unit Price"].ToString(),
                        Discount = reader["Discount"].ToString(),
                        Amount = reader["Amount"].ToString()
                    });
                }
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex) { message = ex.Message; }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBilling(string CUSCOD, string Company, string DueDatMin, string DueDatMax)
        {
            string message = "";
            List<BillingDue> Getdata = new List<BillingDue>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_FindBillingDueNotPayByCus", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Cuscod", CUSCOD);
                command.Parameters.AddWithValue("@Company", Company);
                command.Parameters.AddWithValue("@InDateMin", DueDatMin);
                command.Parameters.AddWithValue("@InDateMax", DueDatMax);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new BillingDue
                    {
                        Company = reader["Company"].ToString(),
                        DocDat = Convert.ToDateTime(reader["DocDat"]).ToString("dd/MM/yyyy"),
                        PstDat = Convert.ToDateTime(reader["PstDat"]).ToString("dd/MM/yyyy"),
                        InvDue = Convert.ToDateTime(reader["InvDue"]).ToString("dd/MM/yyyy"),
                        Invnum = reader["Invnum"].ToString(),
                        Amt = reader["Amt"].ToString(),
                        BillNo = reader["BillNo"].ToString(),
                        BillDat = Convert.ToDateTime(reader["BillDat"]).ToString("dd/MM/yyyy"),
                        BillDue = Convert.ToDateTime(reader["BillingDue"]).ToString("dd/MM/yyyy")
                    });
                }
                message = "Y";
                reader.Close();
                command.Dispose();
                Connection.Close();

            }
            catch (Exception ex) { message = ex.Message; }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BackordYear(string CUSCOD)
        {
            return Json(new { message = "Y", });
        }
    }
}
