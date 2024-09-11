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
    public class PromotionController : Controller
    {
        //
        // GET: /Promotion/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegisterCustomer()
        {
            string user = Session["UserID"].ToString();
            string slmCodeDefault = Session["UserID"].ToString();
            string flagSup = string.Empty;

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
            {
                Connection.Open();
                if (slmCodeDefault != null)
                {
                    SqlCommand cmd1 = new SqlCommand("select TOP 1 * From v_SLMTAB_SM_Userrid where SUP = N'" + slmCodeDefault + "'", Connection);
                    SqlDataReader rev = cmd1.ExecuteReader();
                    while (rev.Read())
                    {
                        flagSup = rev["SUP"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd1.Dispose();
                }
            }

            ViewBag.slmCodeList = GetSalesmanName(user);
            ViewBag.pmCodeList = GetProductName("");
            //ViewBag.slmCode = slmCode == null ? slmCodeDefault : slmCode;
            ViewBag.flagSup = flagSup;

            return View();
        }
        //get name sales
        public List<SelectListItem> GetSalesmanName(string slmCode)
        {
            List<SelectListItem> slmCodeList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Chk_user", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UsrID", slmCode);
            command.Parameters.AddWithValue("@Password", "");
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                slmCodeList.Add(new SelectListItem() { Value = dr["SLMCOD"].ToString(), Text = dr["SLMCOD"].ToString() + "/" + dr["SLMNAM"].ToString() });
            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();

            return slmCodeList;
        }
        public List<SelectListItem> GetProductName(string User)
        {
            List<SelectListItem> productList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Search_Product", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUser", User);
            command.ExecuteNonQuery();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                productList.Add(new SelectListItem() { Value = dr["PROD"].ToString(), Text = dr["PROD"].ToString() + "/" + dr["PRODNAM"].ToString() });

            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();

            return productList;
        }
        //get PromotionCode By Pmcode
        public JsonResult GetPromotion(string prodMgr, string year, string company)
        {
            List<listPromotionCode> promotionList = new List<listPromotionCode>();
            SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString);
            Connection.Open();
            var command = new SqlCommand("P_Search_Promotioncode", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPm", prodMgr);
            command.Parameters.AddWithValue("@inYear", year);
            command.Parameters.AddWithValue("@inCom", company);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                promotionList.Add(new listPromotionCode()
                {
                    PromotionCode = dr["Promotion_code"].ToString(),
                    PromotionDes = dr["Description"].ToString()
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(promotionList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPromotionDetail(string promotionCode, string slmCode)
        {
            string message = "";
            int countList = 0;
            List<listPromotionDetail> promotionDetailList = new List<listPromotionDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Promotion_Detail", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inPromotionCode", promotionCode);
                command.Parameters.AddWithValue("@inSlmCode", slmCode);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ++countList;
                    if (countList == 1) {
                        @ViewBag.RegisterExpireDate = reader["RegisterExpireDate"].ToString();
                    }
                    promotionDetailList.Add(new listPromotionDetail()
                    {
                        Promotion_Code = reader["Promotion_Code"].ToString(),
                        Seq = reader["Seq"].ToString(),
                        Type = reader["Type"].ToString(),
                        DesType = reader["DesType"].ToString(),
                        Description = reader["Description"].ToString(),
                        Condition = reader["Condition"].ToString(),
                        Reward = reader["Reward"].ToString(),
                        Reward_Percent = reader["Reward_Percent"].ToString(),
                        Count_reg = reader["Count_reg"].ToString()
                    });
                }
                //if (Getdata.Any())
                //{
                //    //get data WH, Round, PIDate, StartDelivery, ExptoArrive
                //    foreach (var rowData in Getdata)
                //    {
                //    }
                //}
                @ViewBag.promotionDetailList = promotionDetailList;
                reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            @ViewBag.messageError = message;
            return PartialView("_DetailPromotionRegister", new
            {
                @ViewBag.promotionDetailList,
                @ViewBag.RegisterExpireDate,
                @ViewBag.messageError
            });
        }
        public ActionResult GetCustomerByPromotion(string slmCode, string promotionCode, string promotionSeq, string textHeader)
        {
            int countCustomer = 0;
            int countCustomerReg = 0;
            string message = "Y";
            List<listCustomerRegister> customerRegisterList = new List<listCustomerRegister>();
            var connectionString = ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Customer_Register", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inSlmCode", slmCode);
                command.Parameters.AddWithValue("@inPromotionCode", promotionCode);
                command.Parameters.AddWithValue("@inPromotionSeq", promotionSeq);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ++countCustomer;
                    if (!string.IsNullOrEmpty(reader["FLAG_REG"].ToString()))
                    {
                        ++countCustomerReg;
                    }
                    customerRegisterList.Add(new listCustomerRegister()
                    {
                        Cuscode = reader["CUSCOD"].ToString(),
                        Cusname = reader["CUSNAM"].ToString(),
                        Slmcode = reader["SLMCOD"].ToString(),
                        FlagReg = reader["FLAG_REG"].ToString(),
                        FlagApprove = reader["FLAG_APPROVE"].ToString()
                    });
                }
                @ViewBag.customerRegisterList = customerRegisterList;
                reader.Close();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            @ViewBag.messageError = message;
            @ViewBag.countCustomer = countCustomer;
            @ViewBag.countCustomerReg = countCustomerReg;
            @ViewBag.textHeader = textHeader;
            return PartialView("_ListCustomer", new
            {
                @ViewBag.customerRegisterList,
                @ViewBag.messageError,
                @ViewBag.textHeader,
                @ViewBag.countCustomer,
                @ViewBag.countCustomerReg
            });
        }
        [HttpPost]
        public ActionResult SaveCustomerRegister(string user, string slmCode, string promotionCode, string promotionSeq, string[] cusCode)
        {
            int numSuccess = 0;
            int numError = 0;
            string message = "Y";
            string cusCodArr = "";
            if (cusCode != null)
            {
                cusCodArr = String.Join(",", cusCode.Select(s => "" + s + ""));
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                //foreach (var listData in (List<listCustomerRegisterSave>)request)
                //{
                var command = new SqlCommand("P_Save_Customer_Register", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUser", user);
                command.Parameters.AddWithValue("@inSlmCode", slmCode);
                command.Parameters.AddWithValue("@inPromotionCode", promotionCode);
                command.Parameters.AddWithValue("@inPromotionSeq", Convert.ToInt32(promotionSeq));
                command.Parameters.AddWithValue("@inCustomerCode", cusCodArr);
                SqlParameter returnValue = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValue.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.ExecuteNonQuery();
                message = returnValue.Value.ToString();
                //if (message == "Y")
                //{
                //    ++numSuccess;
                //}
                //else
                //{
                //    ++numError;
                //}
                command.Dispose();
                //}
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                Connection.Close();
            }
            return Json(new { status = message, numSuccess = numSuccess, numError = numError }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApproveChangeCustomer()
        {
            string user = Session["UserID"].ToString();
            string flagSup = string.Empty;
            List<SelectListItem> slmCodeList = new List<SelectListItem>();
            List<SelectListItem> productList = new List<SelectListItem>();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
            {
                Connection.Open();
                //get salesman
                var command = new SqlCommand("P_Price_Approve_Data", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUsrID", user);
                command.Parameters.AddWithValue("@inType", 2);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    slmCodeList.Add(new SelectListItem() { Value = dr["SLMCOD"].ToString(), Text = dr["SLMCOD"].ToString() + "/" + dr["SLMNAM"].ToString() });
                }
                dr.Close();
                command.Dispose();

                command = new SqlCommand("P_Price_Approve_Data", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUsrID", user);
                command.Parameters.AddWithValue("@inType", 4);
                SqlDataReader dr2 = command.ExecuteReader();
                while (dr2.Read())
                {
                    productList.Add(new SelectListItem() { Value = dr2["PROD"].ToString(), Text = dr2["PROD"].ToString() + "/" + dr2["PRODNAM"].ToString() });
                }
                dr2.Close();
                command.Dispose();
            }

            ViewBag.slmCodeList = slmCodeList;
            ViewBag.pmCodeList = productList;
            //ViewBag.slmCode = slmCode == null ? slmCodeDefault : slmCode;
            ViewBag.flagSup = flagSup;

            return View();
        }
        public ActionResult GetPromotionApprove(string slmCode, string company, string year, string prodMgr, string promotionCode)
        {
            string message = "Y";
            string user = Session["UserID"].ToString();
            List<listCustomerApprove> promotionChangeList = new List<listCustomerApprove>();
            var connectionString = ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_Search_Approve_Change_Customer", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inSlmCode", slmCode);
                command.Parameters.AddWithValue("@inCompany", company);
                command.Parameters.AddWithValue("@inProd", prodMgr);
                command.Parameters.AddWithValue("@inPromotionCode", promotionCode);
                command.Parameters.AddWithValue("@inUser", user);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    promotionChangeList.Add(new listCustomerApprove()
                    {
                        Slmcode = dr["SLMCOD"].ToString(),
                        Cuscode = dr["ProCusSelCod"].ToString(),
                        Cusname = dr["CUSNAM"].ToString(),
                        Date_change = dr["Date_change"].ToString(),

                        Promotion_Code_Old = dr["Promotion_Code_old"].ToString(),
                        Promotion_Sub_Old = dr["Promotion_Sub_Old"].ToString(),
                        Description_Old = dr["Description_old"].ToString(),
                        Reward_Old = dr["Reward_old"].ToString(),
                        Cost_Old = dr["Cost_old"].ToString(),

                        Promotion_Code_New = dr["Promotion_Code_new"].ToString(),
                        Promotion_Sub_New = dr["Promotion_Sub_New"].ToString(),
                        Description_New = dr["Description_new"].ToString(),
                        Reward_New = dr["Reward_new"].ToString(),
                        Cost_New = dr["Cost_New"].ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            @ViewBag.messageError = message;
            @ViewBag.promotionChangeList = promotionChangeList;
            //@ViewBag.countCustomerReg = countCustomerReg;
            //@ViewBag.textHeader = textHeader;
            return PartialView("_DetailChangeCustomer", new
            {
                @ViewBag.promotionChangeList,
                @ViewBag.messageError
            });
        }
        [HttpPost]
        public ActionResult SaveApproveChangeCustomer(listSaveCustomerApprove[] requestData, string user, string flagApprove) //(string Cuscode, string Codeold, string Seqold, string Codenew, string Seqnew, string User, string Flag)
        {
            int numSuccess = 0;
            int numError = 0;
            string message = "Y";
            List<listSaveCustomerApprove> promotionChangeList = new List<listSaveCustomerApprove>();
            var connectionString = ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                if (requestData != null)
                {
                    Connection.Open();
                    foreach (var rowData in requestData)
                    {
                        var command = new SqlCommand("P_Save_Approve_Change_Customer", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@inCuscode", rowData.Cuscode);
                        command.Parameters.AddWithValue("@inCodeold", rowData.Codeold);
                        command.Parameters.AddWithValue("@inSeqold", Convert.ToInt32(rowData.Seqold));
                        command.Parameters.AddWithValue("@inCodenew", rowData.Codenew);
                        command.Parameters.AddWithValue("@inSeqnew", Convert.ToInt32(rowData.Seqnew));
                        command.Parameters.AddWithValue("@inUser", user);
                        command.Parameters.AddWithValue("@inFlagApprove", flagApprove);
                        //SqlParameter returnValue = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                        //returnValue.Direction = System.Data.ParameterDirection.Output;
                        //command.Parameters.Add(returnValue);
                        command.ExecuteNonQuery();
                        //message = returnValue.Value.ToString();
                        command.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                Connection.Close();
            }
            return Json(new { status = message, numSuccess = numSuccess, numError = numError }, JsonRequestBehavior.AllowGet);
        }
    }
}
