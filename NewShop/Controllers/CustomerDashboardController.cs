using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
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
using UAParser;
using System.Net;

namespace NewShop.Controllers
{
    public class CustomerDashboardController : Controller
    {
        //
        // GET: /SrcSaleCoCrmStatus/ to CheckStatus / CustomerDashboard

        public ActionResult Index()
        {
            string message = string.Empty;
            //this.Session["UserType"] = "";
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=amount");
            }
            List<SelectListItem> BrandList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                //list brand
                var command = new SqlCommand("p_Search_Brand_Item", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InCompany", "");
                SqlDataReader dr3 = command.ExecuteReader();
                while (dr3.Read())
                {
                    BrandList.Add(new SelectListItem() { Value = dr3["Brand"].ToString(), Text = dr3["Brand"].ToString() });
                }
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            ViewBag.BrandList = BrandList;
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
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=deliveryTrack");
            }
            return View();
        }
        public ActionResult Warranty()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=warranty");

            }
            return View();
        }
        public ActionResult Credit()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("https://mst.aac.co.th/MobileCatalog_Test/Account/CheckLoginExternal?page=menu");
            }
            return View();
        }
        public ActionResult CustomerAlert()
        {
            var uaString = Request.Headers["User-Agent"].ToString();
            var uaParser = Parser.GetDefault();
            string ipAddress = GetIp();
            string macAddress = GetMACAddress();
            ClientInfo clientInfo = uaParser.Parse(uaString);

            var os = clientInfo.OS.ToString();
            var browser = clientInfo.UserAgent.ToString();

            ViewBag.OS = os;
            ViewBag.Browser = browser;
            ViewBag.Mac = macAddress;
            ViewBag.IP = ipAddress;
            return View();
        }
        public string GetIp()
        {
            string ip =
            System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public string GetMACAddress()
        {
            string macAddresses = "";

            foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        private string GetMacAddress()
        {
            string macAddress = NetworkInterface
                                .GetAllNetworkInterfaces()
                                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                .Select(nic => nic.GetPhysicalAddress().ToString())
                                .FirstOrDefault();
            return macAddress;
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
                        LASIVC = reader["LASIVC"] != DBNull.Value ? Convert.ToDateTime(reader["LASIVC"]).ToString("dd/MM/yy") : "",
                        AACCRLINE = reader["AACCRLINE"].ToString(),
                        AACBAL = reader["AACBAL"].ToString(),
                        AACBALDue = reader["AACBALDue"].ToString(),
                        AACBilDue = reader["AACBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["AACBilDue"]).ToString("dd/MM/yy") : "",
                        TACCRLINE = reader["TACCRLINE"].ToString(),
                        TACBAL = reader["TACBAL"].ToString(),
                        TACBALDue = reader["TACBALDue"].ToString(),
                        TACBilDue = reader["TACBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["TACBilDue"]).ToString("dd/MM/yy") : "",
                        OMPCRLINE = reader["OMPCRLINE"].ToString(),
                        OMPBAL = reader["OMPBAL"].ToString(),
                        OMPBALDue = reader["OMPBALDue"].ToString(),
                        OMPBilDue = reader["OMPBilDue"] != DBNull.Value ? Convert.ToDateTime(reader["OMPBilDue"]).ToString("dd/MM/yy") : "",
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
        public JsonResult getbackorder_S_notify(string CUSCOD)
        {
            string message = "";
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<BackOrder_Notify> backorder = new List<BackOrder_Notify>();
            try
            {
                var command = new SqlCommand("P_Search_BackOrder_S_Notify", Connection);
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
                        SaleOrderDate = Convert.ToDateTime(reader["SaleOrder_Date"]).ToString("dd/MM/yy"),
                        DeliveryDate = reader["DeliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["SaleOrder_Date"]).ToString("dd/MM/yy") : ""
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
                        SaleOrderDate = Convert.ToDateTime(reader["SaleOrder_Date"]).ToString("dd/MM/yy"),
                        DeliveryDate = reader["DeliveryDate"] != DBNull.Value ? reader["DeliveryDate"].ToString() : "",
                        Note = reader["Note"] != DBNull.Value ? reader["Note"].ToString() : ""

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
        public JsonResult GetDeliveryDetail(string ORD_DocNo)
        {
            string message = "";
            List<DeliveryTrackNotify_Detail> Getdata = new List<DeliveryTrackNotify_Detail>();
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
                    Getdata.Add(new DeliveryTrackNotify_Detail()
                    {
                        RowNo = reader["RowNo"].ToString(),
                        ORD_STKCOD = reader["ORD_STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        ORD_STKGRP = reader["ORD_STKGRP"].ToString(),
                        ORD_Price = reader["ORD_Price"].ToString(),
                        ORD_SalePrice = reader["ORD_SalePrice"].ToString(),
                        ORD_Date = reader["ORD_Date"].ToString(),
                        Item_Type = reader["Item_Type"].ToString(),
                        ORD_Discount = reader["ORD_Discount"].ToString(),
                        ORD_Qty = reader["ORD_Qty"].ToString(),
                        ORD_Amt = reader["ORD_Amt"].ToString(),
                        BCK_Qty = reader["BCK_Qty"].ToString(),
                        BackOrder = reader["BackOrder"].ToString()
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
        public JsonResult GetAlertDeliveryTracking(string CUSCOD)
        {
            string message = "";
            var Getdata = new List<object>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Order_Notify_Detail_Count", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD);
                SqlDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Getdata.Add(new
                    {
                        Order = Reader["รับOrder"],
                        Pack = Reader["เตรียมจัดส่ง"],
                        Deliver = Reader["ระหว่างขนส่ง"],
                        Arrive = Reader["จัดส่งสำเร็จ"]
                    });
                }
                message = "Y";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        //Tracking
        public JsonResult GetDeliveryTracking(string CUSCOD, string NotifyID)
        {
            string message = "";
            List<Notify_Detail> Getdata = new List<Notify_Detail>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Order_Notify_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 120;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD.Trim());
                command.Parameters.AddWithValue("@NotifyID", NotifyID);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Notify_Detail()
                    {
                        ORD_DocNo = reader["ORD_DocNo"].ToString(),
                        NotifyID = reader["NotifyID"].ToString(),
                        Notify = reader["Notify"].ToString(),
                        StatusDate = reader["StatusDate"] != DBNull.Value ? DateTime.Parse(reader["StatusDate"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        PIDate = reader["PIDate"] != DBNull.Value ? DateTime.Parse(reader["PIDate"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        ExpToArrive = reader["ExpToArrive"] != DBNull.Value ? DateTime.Parse(reader["ExpToArrive"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        StartDelivery = reader["StartDelivery"] != DBNull.Value ? DateTime.Parse(reader["StartDelivery"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        EndDelivery = reader["EndDelivery"] != DBNull.Value ? DateTime.Parse(reader["EndDelivery"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        OrderDate = reader["Order Date"] != DBNull.Value ? DateTime.Parse(reader["Order Date"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        ORD_TotalItem = reader["ORD_TotalItem"].ToString(),
                        ORD_TotalQty = reader["ORD_TotalQty"].ToString(),
                        ORD_TotalAmt = reader["ORD_TotalAmt"].ToString(),
                        Round = reader["Round"].ToString(),
                        WH = reader["WH"].ToString(),
                        ShipTo = reader["ShipTo"].ToString(),
                        Item_Type = reader["Item_Type"].ToString(),
                        ORD_STKCOD = reader["ORD_STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        ORD_SalePrice = reader["ORD_SalePrice"].ToString(),
                        ORD_Qty = reader["ORD_Qty"].ToString(),
                        ORD_Amt = reader["ORD_Amt"].ToString(),
                        BCK_Qty = reader["BCK_Qty"].ToString(),
                        BackOrder = reader["BackOrder"].ToString()
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

        public JsonResult GetDeliveryTrackingSuccess(string CUSCOD, string DATE)
        {
            string message = "";
            List<Notify_Detail> Getdata = new List<Notify_Detail>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Order_Notify_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 120;
                command.Parameters.AddWithValue("@CUSCOD", CUSCOD.Trim());
                command.Parameters.AddWithValue("@NotifyID", "4");
                command.Parameters.AddWithValue("@OrdDat", DATE);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Notify_Detail()
                    {
                        ORD_DocNo = reader["ORD_DocNo"].ToString(),
                        NotifyID = reader["NotifyID"].ToString(),
                        Notify = reader["Notify"].ToString(),
                        StatusDate = reader["StatusDate"] != DBNull.Value ? DateTime.Parse(reader["StatusDate"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        PIDate = reader["PIDate"] != DBNull.Value ? DateTime.Parse(reader["PIDate"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        ExpToArrive = reader["ExpToArrive"] != DBNull.Value ? DateTime.Parse(reader["ExpToArrive"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        StartDelivery = reader["StartDelivery"] != DBNull.Value ? DateTime.Parse(reader["StartDelivery"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        EndDelivery = reader["EndDelivery"] != DBNull.Value ? DateTime.Parse(reader["EndDelivery"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        OrderDate = reader["Order Date"] != DBNull.Value ? DateTime.Parse(reader["Order Date"].ToString()).ToString("yy-MM-dd HH:mm") : string.Empty,
                        ORD_TotalItem = reader["ORD_TotalItem"].ToString(),
                        ORD_TotalQty = reader["ORD_TotalQty"].ToString(),
                        ORD_TotalAmt = reader["ORD_TotalAmt"].ToString(),
                        Round = reader["Round"].ToString(),
                        WH = reader["WH"].ToString(),
                        ShipTo = reader["ShipTo"].ToString(),
                        Item_Type = reader["Item_Type"].ToString(),
                        ORD_STKCOD = reader["ORD_STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        ORD_SalePrice = reader["ORD_SalePrice"].ToString(),
                        ORD_Qty = reader["ORD_Qty"].ToString(),
                        ORD_Amt = reader["ORD_Amt"].ToString(),
                        BCK_Qty = reader["BCK_Qty"].ToString(),
                        BackOrder = reader["BackOrder"].ToString()
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

        //get order detail notify in line
        [HttpGet]
        public ActionResult GetDeliveryDetailByDocnoParams()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetDeliveryDetailByDocno()
        {
            //string getDocno = string.Empty;
            string setDocno = string.Empty;
            string encodeDocno = string.Empty;
            string getDocno = string.Empty;

            getDocno = Request.Params["getDocno"];
            if (getDocno != null)
            {
                byte[] data = System.Convert.FromBase64String(getDocno);
                setDocno = System.Text.ASCIIEncoding.ASCII.GetString(data);
            }
            @ViewBag.Docno = setDocno;

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("STC24030886");
            encodeDocno = System.Convert.ToBase64String(plainTextBytes);

            string message = "";
            List<SaleOrderDetail> Getdata = new List<SaleOrderDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Order_Notify_List_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inORD_DocNo", setDocno);
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
                        BckQty = reader["BCK_Qty"].ToString(),
                        FlagBackOrder = reader["BackOrder"].ToString(),
                        TotalAmt = reader["ORD_Amt"].ToString()
                    });
                }
                @ViewBag.Getdata = Getdata;
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return PartialView("_DeliveryDetailByDocno", new
            {
                @ViewBag.Getdata,
                @ViewBag.Docno
            });
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
                        PSTDAT = Convert.ToDateTime(reader["PSTDAT"]).ToString("dd/MM/yy"),
                        DUEDAT = Convert.ToDateTime(reader["DUEDAT"]).ToString("dd/MM/yy"),
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
                        DocDat = Convert.ToDateTime(reader["DocDat"]).ToString("dd/MM/yy"),
                        PstDat = Convert.ToDateTime(reader["PstDat"]).ToString("dd/MM/yy"),
                        InvDue = Convert.ToDateTime(reader["InvDue"]).ToString("dd/MM/yy"),
                        Invnum = reader["Invnum"].ToString(),
                        Amt = reader["Amt"].ToString(),
                        BillNo = reader["BillNo"].ToString(),
                        BillDat = Convert.ToDateTime(reader["BillDat"]).ToString("dd/MM/yy"),
                        BillDue = Convert.ToDateTime(reader["BillingDue"]).ToString("dd/MM/yy")
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

        //promotion
        public JsonResult GetYearPromotion(string CUSCOD)
        {
            string message = "";
            List<PromotionYearList> Getdata = new List<PromotionYearList>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Promotion_Cus_Notify", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new PromotionYearList
                    {
                        Year = reader["Year"].ToString(),
                        Reward_Amount = reader["Reward Amount"].ToString(),
                        Ticket = reader["Ticket"].ToString(),
                        Paid_Amount = reader["Paid Amount"].ToString(),
                        Waiting_Amount = reader["Waiting Amount"].ToString(),
                        WHT = reader["WHT"].ToString()
                    });
                }
                message = "Y";
                reader.Close();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPromotionDetail(string CUSCOD, string Period, string YEAR)
        {
            string message = "";
            List<Promotion_CusList> Getdata = new List<Promotion_CusList>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Promotion_Cus", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@inPeriod", Period);
                command.Parameters.AddWithValue("@inYear", YEAR);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Promotion_CusList
                    {
                        Company = reader["company"].ToString(),
                        Promotion_Code = reader["Promotion_Code"].ToString(),
                        Promotion_Name = reader["Promotion_Name"].ToString(),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        Condition = reader["Condition"].ToString(),
                        Invoice_Amount = reader["Invoice Amount"].ToString(),
                        Invoice_Paid = reader["Invoice Paid"].ToString(),
                        Remaining_Amount = reader["Remaining Amount"].ToString(),
                        Reward = reader["Reward"].ToString(),
                        Reward_Amt = reader["Reward_Amt"].ToString(),
                        Received_By = reader["Received_By"].ToString(),
                        Received_Date = reader["Received_date"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["Received_date"]).ToString("dd/MM/yy") : "",
                        WHT = reader["WHT"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
                message = "Y";
                reader.Close();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailPromotion(string CUSCOD, string proCODE)
        {
            string message = "";
            List<Promotion_Detail> Getdata = new List<Promotion_Detail>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Promotion_Cus_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@inPromotion_Code", proCODE);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new Promotion_Detail
                    {
                        Promotion_code = reader["Promotion_code"].ToString(),
                        Description = reader["Description"].ToString(),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        Company = reader["Company"].ToString(),
                        DOCNUM = reader["DOCNUM"].ToString(),
                        DOCDAT = Convert.ToDateTime(reader["DOCDAT"]).ToString("dd/MM/yy"),
                        STKCOD = reader["STKCOD"].ToString(),
                        STKDES = reader["STKDES"].ToString(),
                        PEOPLE = reader["PEOPLE"].ToString(),
                        QTY = reader["QTY"].ToString(),
                        SP_LCY = reader["SP_LCY"].ToString(),
                        NET_LCY = reader["NET_LCY"].ToString(),
                        CMPCHK = reader["CMPCHK"].ToString(),
                        CMPLDAT = reader["CMPLDAT"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["CMPLDAT"]).ToString("dd/MM/yy") : "",
                    });
                }
                message = "Y";
                reader.Close();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderHistoryByBrand(string cuscod, string brand)
        {
            string message = string.Empty;

            List<OrderHistoryByBrand> Getdata = new List<OrderHistoryByBrand>();
            List<SelectListItem> BrandList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("p_Search_XCusItm", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InCuscod", cuscod);
                command.Parameters.AddWithValue("@InBrand", brand);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new OrderHistoryByBrand()
                    {
                        Company = reader["Company"].ToString(),
                        Stkcod = reader["STKCOD"].ToString(),
                        Stkdes = reader["STKDES"].ToString(),
                        year_1_qty = reader["year_1_qty"].ToString(),
                        year_1_amt = reader["year_1_amt"].ToString(),
                        year_1_avg = reader["year_1_avg"].ToString(),
                        year_2_qty = reader["year_2_qty"].ToString(),
                        year_2_amt = reader["year_2_amt"].ToString(),
                        year_2_avg = reader["year_2_avg"].ToString(),
                        year_3_qty = reader["year_3_qty"].ToString(),
                        year_3_amt = reader["year_3_amt"].ToString(),
                        year_3_avg = reader["year_3_avg"].ToString(),
                        year_4_qty = reader["year_4_qty"].ToString(),
                        year_4_amt = reader["year_4_amt"].ToString(),
                        year_4_avg = reader["year_4_avg"].ToString(),
                        Brand = reader["Brand"].ToString(),
                        Prclist = reader["Prclist"].ToString()
                    });
                }

                //list brand
                command = new SqlCommand("p_Search_Brand_Item", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InCompany", "");
                SqlDataReader dr3 = command.ExecuteReader();
                while (dr3.Read())
                {
                    BrandList.Add(new SelectListItem() { Value = dr3["Brand"].ToString(), Text = dr3["Brand"].ToString() });

                }
                ViewBag.BrandList = BrandList;
                ViewBag.Getdata = Getdata;
                ViewBag.YearCurrent = DateTime.Now.Year.ToString();
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return PartialView("_OrderHistoryByBrand", new
            {
                ViewBag.Getdata,
                ViewBag.YearCurrent,
                ViewBag.BrandList
            });
        }
        public ActionResult MenuTest()
        {
            return View("MenuTest");
        }
    }
}
