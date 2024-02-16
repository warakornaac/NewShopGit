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
    public class PriceApprovalController : Controller
    {
        //
        // GET: /PriceApproval/

        public ActionResult Index()
        {

            if (this.Session["UserType"] == "")
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                if (this.Session["UserType"] == null)
                {
                    return RedirectToAction("LogIn", "Account");
                }
                else
                {
                    string usre = Session["UserID"].ToString();
                    //string Password = Session["UserPassword"].ToString();
                    //var dd = db.UsrTbl.Where(c => c.UsrID == usre).ToArray();
                    //if (dd.Length == 1)
                    //{
                    //    if (dd[0].UsrTyp == 1)
                    //    {
                    //        return RedirectToAction("bysaleco", "PriceApproval");
                    //    }
                    //}
                    //else
                    //{
                    //    return RedirectToAction("bysale", "PriceApproval");
                    //}
                    //ViewBag.usre = usre;
                    // FormsAuthentication.SetAuthCookie(User.usre, false);
                    //return RedirectToAction("Index", "SeleScrCustomer");
                    List<SLM> SlmList = new List<SLM>();
                    List<SelectListItem> GroupStkGrp = new List<SelectListItem>();
                    List<SelectListItem> PRODList = new List<SelectListItem>();
                    using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
                    {
                        Connection.Open();

                        var command = new SqlCommand("P_Price_Approve_Data", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@inUsrID", usre);
                        command.Parameters.AddWithValue("@inType", 2);
                        // command.ExecuteNonQuery();
                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            SlmList.Add(new SLM()
                            {
                                SLMCOD = dr["SLMCOD"].ToString(),
                                SLMNAM = dr["SLMNAM"].ToString()
                            });
                        }
                        ViewBag.SlmModel = SlmList;
                        //dr.Dispose();
                        //S20161016
                        dr.Close();
                        dr.Dispose();
                        command.Dispose();
                        //E20161016

                        command = new SqlCommand("P_Price_Approve_Data", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@inUsrID", usre);
                        command.Parameters.AddWithValue("@inType", 3);
                        //command.ExecuteNonQuery();
                        SqlDataReader dr2 = command.ExecuteReader();
                        while (dr2.Read())
                        {
                            GroupStkGrp.Add(new SelectListItem() { Value = dr2["STKGRP"].ToString(), Text = dr2["STKGRP"].ToString() + "/" + dr2["GRPNAM"].ToString() });

                        }
                        ViewBag.StkGrp = GroupStkGrp;
                        //dr2.Dispose();
                        //S20161016
                        dr2.Close();
                        dr2.Dispose();
                        command.Dispose();
                        //E20161016

                        command = new SqlCommand("P_Price_Approve_Data", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@inUsrID", usre);
                        command.Parameters.AddWithValue("@inType", 4);
                        //command.ExecuteNonQuery();
                        SqlDataReader dr3 = command.ExecuteReader();
                        while (dr3.Read())
                        {
                            PRODList.Add(new SelectListItem() { Value = dr3["PROD"].ToString(), Text = dr3["PROD"].ToString() + "/" + dr3["PRODNAM"].ToString() });

                        }
                        ViewBag.PRODList = PRODList;
                        //dr3.Dispose();
                        //S20161016
                        dr3.Close();
                        dr3.Dispose();
                        command.Dispose();
                        //E20161016
                        Connection.Close();
                    }

                }
            }
            return View();
        }
       
       
        public JsonResult GetdataCus(string SLM, string SLMNAME)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            SqlCommand cmd;
            if (SLM != "")
            {
                cmd = new SqlCommand("select * from v_CUSPROV where SLMCOD =N'" + SLM + "'", Connection);
            }
            else
            {
                cmd = new SqlCommand("select * from v_CUSPROV", Connection);
            }
            //else
            //{
            // SqlCommand cmd = new SqlCommand("select * from v_CUSPROV ", Connection);
            //}
            //  this.Session["SLM"] = SLM;
            //  this.Session["SLMCOD"] = SLMNAME;
            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                CUSList.Add(new CUS()
                {
                    CUSCOD = rev_CUSPROV.GetValue(1).ToString(),
                    CUSNAM = rev_CUSPROV.GetValue(2).ToString()
                });
            }
            //rev_CUSPROV.Dispose();
            //S20161016
            rev_CUSPROV.Close();
            rev_CUSPROV.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(CUSList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSTKGRP(string ProdMRG, string ProdMRG_NAME)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<STKGRPList> STKGRPList = new List<STKGRPList>();

            SqlCommand cmd = new SqlCommand("P_Price_Approve_Data  @inType=5,@inProd =N'" + ProdMRG + "'", Connection);

            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                STKGRPList.Add(new STKGRPList()
                {
                    STKGRP = rev_CUSPROV["STKGRP"].ToString(),
                    GRPNAM = rev_CUSPROV["GRPNAM"].ToString()
                });
            }
            //rev_CUSPROV.Dispose();
            //S20161016
            rev_CUSPROV.Close();
            rev_CUSPROV.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(STKGRPList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetPriceApprove_Group(string CartID)
        {
            int sumQty = 0;
            int sumSalePrice = 0;
            //int sumDiscount = 0;
            List<ItemList_PriceApprove> Getdata = new List<ItemList_PriceApprove>();
            ItemPriceApprove model = null;

            string usre = Session["UserID"].ToString();


            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
            {
                Connection.Open();
                var command = new SqlCommand("P_Search_Approval_Group_catalog", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCartID", CartID);

                //command.ExecuteNonQuery();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    model = new ItemPriceApprove();
                    DateTime date = Convert.ToDateTime(dr["ORDDAT"].ToString());
                    string formatted = date.ToString("dd/M/yyyy");
                    string Ldate = dr["LastInvdate"].ToString();
                    if (Ldate != "")
                    {
                        DateTime dateLastInvdate = Convert.ToDateTime(dr["LastInvdate"].ToString());
                        string formattedLastInvdate = dateLastInvdate.ToString("dd/M/yyyy");
                        model.LastInvdate = formattedLastInvdate;
                    }
                    else
                    {
                        model.LastInvdate = "-";

                    }
                    model.RowNo = dr["RowNo"].ToString();

                    model.SODate = dr["SODate"].ToString();
                    model.SONumber = dr["SONumber"].ToString();

                    model.ORDDAT = dr["ORDDAT"].ToString();
                    model.StatusFull = dr["StatusFull"].ToString();
                    model.PrcApproveDate = dr["PrcApproveDate"].ToString();
                    model.ID = dr["ID"].ToString();
                    model.PRCLST_NO = dr["PRCLST_NO"].ToString();
                    //    model.Status = dr["Status"].ToString();
                    model.CUSCOD = dr["CUSNAM"].ToString();
                    model.SLMID = dr["SLMNAM"].ToString();

                    model.ORDDAT = formatted;
                    model.STKCOD = dr["STKCOD"].ToString();
                    model.STKGRP = dr["STKGRP"].ToString();
                    model.STKDES = dr["STKDES"].ToString();
                    model.MINORD = dr["MINORD"].ToString();
                    model.Price = dr["Price"].ToString();
                    model.SalePrice = dr["SalePrice"].ToString();
                    model.SpecialPrice = dr["SpecialPrice"].ToString();
                    model.spcmoq = dr["spc_moq"].ToString();
                    // model.spcstart_date = dr["spc_start_date"].ToString();
                    // model.spcend_date = dr["spc_end_date"].ToString();
                    string spc_start_date = dr["spc_start_date"].ToString();
                    if (spc_start_date != "")
                    {
                        DateTime spc_s_date = Convert.ToDateTime(dr["spc_start_date"].ToString());
                        string formatspc_s_date = spc_s_date.ToString("dd/MM/yyyy");
                        model.spcstart_date = formatspc_s_date;
                    }
                    else
                    {
                        model.spcstart_date = dr["spc_start_date"].ToString();
                    }
                    //Model.spc_end_date = dr["spc_end_date"].ToString(); 	
                    string Ldate_end_dat = dr["spc_end_date"].ToString();
                    if (Ldate_end_dat != "")
                    {
                        DateTime dateend_dat = Convert.ToDateTime(dr["spc_end_date"].ToString());
                        string formatted_dateend_dat = dateend_dat.ToString("dd/MM/yyyy");
                        model.spcend_date = formatted_dateend_dat;

                    }
                    else
                    {
                        model.spcend_date = "-";

                    }
                    model.Qty = dr["Qty"].ToString();
                    model.Amt = dr["Amt"].ToString();
                    model.Discount = dr["Discount"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.ExpectPrice = dr["ExpectPrice"].ToString();
                    //model.MINORD = dr["MINORD"].ToString();
                    model.UOM = dr["UOM"].ToString();
                    model.LastInvPrice = dr["LastInvPrice"].ToString();

                    model.Promotion = dr["Promotion"].ToString();
                    model.PromotionDesc = dr["PromotionDesc"].ToString();
                    // sumQty += Convert.ToInt32(dr.GetValue(15));
                    // sumSalePrice += Convert.ToInt32(dr.GetValue(9));
                    Getdata.Add(new ItemList_PriceApprove { val = model });

                }
                //S20161016
                dr.Close();
                dr.Dispose();
                command.Dispose();
                //E20161016
                Connection.Close();
            }

            return Json(new { Getdata, sumQty, sumSalePrice }, JsonRequestBehavior.AllowGet);
        }
     
        public JsonResult GetPriceApprove(string CUSCOD, string SLMCODE, string STKGRP, string ProdMRG, string vStatus, string Usre)
        {
            int sumQty = 0;
            int sumSalePrice = 0;
            //  int sumDiscount = 0;
            List<ItemList_PriceApprove> Getdata = new List<ItemList_PriceApprove>();
            ItemPriceApprove model = null;

            string usre = Usre;


            if (ProdMRG != "0")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
                {
                    Connection.Open();
                    //var command = new SqlCommand("P_Search_Approval", Connection);
                    var command = new SqlCommand("P_Search_Approval_catalog", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inSLMCODE", SLMCODE);
                    command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                    command.Parameters.AddWithValue("@inSTKGRP", STKGRP);
                    command.Parameters.AddWithValue("@inProd", ProdMRG);
                    command.Parameters.AddWithValue("@inStatus", vStatus);
                    command.Parameters.AddWithValue("@inUsrID", usre);
                    //command.ExecuteNonQuery();
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        model = new ItemPriceApprove();
                        DateTime date = Convert.ToDateTime(dr["ORDDAT"].ToString());
                        string formatted = date.ToString("dd/M/yyyy");
                        string Ldate = dr["LastInvdate"].ToString();
                        if (Ldate != "")
                        {
                            DateTime dateLastInvdate = Convert.ToDateTime(dr["LastInvdate"].ToString());
                            string formattedLastInvdate = dateLastInvdate.ToString("dd/M/yyyy");
                            model.LastInvdate = formattedLastInvdate;
                        }
                        else
                        {
                            model.LastInvdate = "-";

                        }
                        model.RowNo = dr["RowNo"].ToString();
                        model.ORDDAT = dr["ORDDAT"].ToString();
                        model.StatusFull = dr["StatusFull"].ToString();
                        model.PrcApproveDate = dr["PrcApproveDate"].ToString();
                        //model.spcstart_date = dr["spc_start_date"].ToString();
                        // model.spcend_date = dr["spc_end_date"].ToString();
                        string spc_start_date = dr["spc_start_date"].ToString();
                        if (spc_start_date != "")
                        {
                            DateTime spc_s_date = Convert.ToDateTime(dr["spc_start_date"].ToString());
                            string formatspc_s_date = spc_s_date.ToString("dd/MM/yyyy");
                            model.spcstart_date = formatspc_s_date;
                            if (vStatus == "N")
                            {
                                model.spcstart_date = dr["PrcApproveDate"].ToString();
                            }
                            else
                            {
                                model.spcstart_date = formatspc_s_date;
                            }
                        }
                        else
                        {
                            model.spcstart_date = dr["spc_start_date"].ToString();
                        }
                        //Model.spc_end_date = dr["spc_end_date"].ToString(); 	
                        string Ldate_end_dat = dr["spc_end_date"].ToString();
                        if (Ldate_end_dat != "")
                        {
                            DateTime dateend_dat = Convert.ToDateTime(dr["spc_end_date"].ToString());
                            string formatted_dateend_dat = dateend_dat.ToString("dd/MM/yyyy");
                            model.spcend_date = formatted_dateend_dat;


                        }
                        else
                        {
                            model.spcend_date = "-";

                        }
                        // model.PrcApproveDate = dr["PrcApproveDate"].ToString();
                        model.ID = dr["ID"].ToString();
                        model.PRCLST_NO = dr["PRCLST_NO"].ToString();
                        //    model.Status = dr["Status"].ToString();
                        model.CUSCOD = dr["CUSNAM"].ToString();
                        model.SLMID = dr["SLMNAM"].ToString();

                        model.ORDDAT = formatted;
                        model.STKCOD = dr["STKCOD"].ToString();
                        model.STKGRP = dr["STKGRP"].ToString();
                        model.STKDES = dr["STKDES"].ToString();
                        model.MINORD = dr["MINORD"].ToString();
                        model.Price = dr["Price"].ToString();
                        model.SalePrice = dr["SalePrice"].ToString();
                        model.SpecialPrice = dr["SpecialPrice"].ToString();
                        model.spcmoq = dr["spc_moq"].ToString();
                        model.Qty = dr["Qty"].ToString();
                        model.Amt = dr["Amt"].ToString();
                        model.Discount = dr["Discount"].ToString();
                        model.Status = dr["Status"].ToString();
                        model.ExpectPrice = dr["ExpectPrice"].ToString();

                        model.UOM = dr["UOM"].ToString();
                        model.LastInvPrice = dr["LastInvPrice"].ToString();

                        model.Promotion = dr["Promotion"].ToString();
                        model.PromotionDesc = dr["PromotionDesc"].ToString();
                        model.Prcdes = dr["PRCDES"].ToString();
                        model.ID = dr["ID"].ToString();
                        model.InsertedBy = dr["Inserted By"].ToString();
                        model.InsertedDate = dr["Inserted Date"].ToString();
                        model.LineNote = dr["LineNote"].ToString();
                        model.UNITCOST = dr["UNITCOST"].ToString();
                        model.readyQty = dr["readyQty"].ToString();
                        // sumQty += Convert.ToInt32(dr.GetValue(15));
                        // sumSalePrice += Convert.ToInt32(dr.GetValue(9));
                        Getdata.Add(new ItemList_PriceApprove { val = model });

                    }
                    //S20161016
                    dr.Close();
                    dr.Dispose();
                    command.Dispose();
                    //E20161016
                    //Connection.Dispose();
                    Connection.Close();
                }

            }
            else
            {

                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
                {
                    Connection.Open();
                    //var command = new SqlCommand("P_Search_Approval", Connection);
                    var command = new SqlCommand("P_Search_Approval_catalog", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inSLMCODE", SLMCODE);
                    command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                    command.Parameters.AddWithValue("@inSTKGRP", STKGRP);
                    command.Parameters.AddWithValue("@inProd", "");
                    command.Parameters.AddWithValue("@inStatus", vStatus);
                    command.Parameters.AddWithValue("@inUsrID", usre);
                    //command.ExecuteNonQuery();
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        model = new ItemPriceApprove();
                        DateTime date = Convert.ToDateTime(dr["ORDDAT"].ToString());
                        string formatted = date.ToString("dd/M/yyyy");
                        string Ldate = dr["LastInvdate"].ToString();
                        if (Ldate != "")
                        {
                            DateTime dateLastInvdate = Convert.ToDateTime(dr["LastInvdate"].ToString());
                            string formattedLastInvdate = dateLastInvdate.ToString("dd/M/yyyy");
                            model.LastInvdate = formattedLastInvdate;
                        }
                        else
                        {
                            model.LastInvdate = "-";

                        }
                        model.RowNo = dr["RowNo"].ToString();
                        model.ORDDAT = dr["ORDDAT"].ToString();
                        model.StatusFull = dr["StatusFull"].ToString();
                        model.PrcApproveDate = dr["PrcApproveDate"].ToString();
                        model.ID = dr["ID"].ToString();
                        model.PRCLST_NO = dr["PRCLST_NO"].ToString();
                        //    model.Status = dr["Status"].ToString();
                        model.CUSCOD = dr["CUSNAM"].ToString();
                        model.SLMID = dr["SLMNAM"].ToString();

                        model.ORDDAT = formatted;
                        model.STKCOD = dr["STKCOD"].ToString();
                        model.STKGRP = dr["STKGRP"].ToString();
                        model.STKDES = dr["STKDES"].ToString();
                        model.MINORD = dr["MINORD"].ToString();
                        model.Price = dr["Price"].ToString();
                        model.SalePrice = dr["SalePrice"].ToString();
                        model.SpecialPrice = dr["SpecialPrice"].ToString();
                        model.MINORD = dr["MINORD"].ToString();
                        model.spcmoq = dr["spc_moq"].ToString();
                        model.spcstart_date = dr["spc_start_date"].ToString();
                        model.spcend_date = dr["spc_end_date"].ToString();
                        string spc_start_date = dr["spc_start_date"].ToString();
                        if (spc_start_date != "")
                        {
                            DateTime spc_s_date = Convert.ToDateTime(dr["spc_start_date"].ToString());
                            string formatspc_s_date = spc_s_date.ToString("dd/MM/yyyy");
                            model.spcstart_date = formatspc_s_date;
                            if (vStatus == "N")
                            {
                                model.spcstart_date = dr["PrcApproveDate"].ToString();
                            }
                            else
                            {
                                model.spcstart_date = formatspc_s_date;
                            }
                        }
                        else
                        {
                            model.spcstart_date = dr["spc_start_date"].ToString();
                        }
                        //Model.spc_end_date = dr["spc_end_date"].ToString(); 	
                        string Ldate_end_dat = dr["spc_end_date"].ToString();
                        if (Ldate_end_dat != "")
                        {
                            DateTime dateend_dat = Convert.ToDateTime(dr["spc_end_date"].ToString());
                            string formatted_dateend_dat = dateend_dat.ToString("dd/MM/yyyy");
                            model.spcend_date = formatted_dateend_dat;


                        }
                        else
                        {
                            model.spcend_date = "-";

                        }
                        model.Qty = dr["Qty"].ToString();
                        model.Amt = dr["Amt"].ToString();
                        model.Discount = dr["Discount"].ToString();
                        model.Status = dr["Status"].ToString();
                        model.ExpectPrice = dr["ExpectPrice"].ToString();

                        model.UOM = dr["UOM"].ToString();
                        model.LastInvPrice = dr["LastInvPrice"].ToString();

                        model.Promotion = dr["Promotion"].ToString();
                        model.PromotionDesc = dr["PromotionDesc"].ToString();
                        model.Prcdes = dr["PRCDES"].ToString();
                        model.ID = dr["ID"].ToString();
                        model.InsertedBy = dr["Inserted By"].ToString();
                        model.InsertedDate = dr["Inserted Date"].ToString();
                        model.LineNote = dr["LineNote"].ToString();
                        model.UNITCOST = dr["UNITCOST"].ToString();
                        // sumQty += Convert.ToInt32(dr.GetValue(15));
                        // sumSalePrice += Convert.ToInt32(dr.GetValue(9));
                        Getdata.Add(new ItemList_PriceApprove { val = model });

                    }
                    //S20161016
                    dr.Close();
                    dr.Dispose();
                    command.Dispose();
                    //E20161016
                    Connection.Close();
                }

            }

            return Json(new { Getdata, sumQty, sumSalePrice }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Approvalndata(string data, string User, string vStatus)
        {
            string message = "false";
            string messagereturnsql = string.Empty;
            string messageerror = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;
            List<Itemapproval> _ItemList = new JavaScriptSerializer().Deserialize<List<Itemapproval>>(data);
            int cid = 0;
            int Smoq = 0;
            DateTime SpcEndDate = new DateTime();
            string strspcend = string.Empty;
            string strSmoq = string.Empty;
            string usre = Session["UserID"].ToString();
            string strend = string.Empty;
            string stremake = string.Empty;
            string messagereturn = string.Empty;
            try
            {
                if (_ItemList.Count > 0)
                {

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        for (int i = 0; i < _ItemList.Count; i++)
                        {
                            cid = Convert.ToInt32(_ItemList[i].cartid);
                            strSmoq = _ItemList[i].spcmoq;
                            if (strSmoq != "")
                            {
                                Smoq = Convert.ToInt32(_ItemList[i].spcmoq);
                            }
                            strspcend = _ItemList[i].spcend_date;
                            if (strspcend != "")
                            {
                                SpcEndDate = DateTime.ParseExact(strspcend, "dd/MM/yyyy", null);

                                //Convert.ToDateTime(_ItemList[i].spcend_date);
                                //SpcEndDate = _ItemList[i].spcend_date;
                            }
                            stremake = _ItemList[i].remake;
                            //SqlCommand command = new SqlCommand("P_Save_Price_Approve", connection);
                            SqlCommand command = new SqlCommand("P_Save_Price_Approve_catalog", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            if (vStatus != "C")
                            {

                                command.Parameters.AddWithValue("@inORDID", cid);
                                command.Parameters.AddWithValue("@inUser", usre);
                                command.Parameters.AddWithValue("@inStatus", vStatus);
                                command.Parameters.AddWithValue("@inSpcmoq", Smoq);
                                command.Parameters.AddWithValue("@inSpcEnddate", SpcEndDate);
                                command.Parameters.AddWithValue("@inRemake", stremake);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@inORDID", cid);
                                command.Parameters.AddWithValue("@inUser", usre);
                                command.Parameters.AddWithValue("@inStatus", vStatus);
                                command.Parameters.AddWithValue("@inSpcmoq", 1);
                                command.Parameters.AddWithValue("@inSpcEnddate", DateTime.Now);
                                command.Parameters.AddWithValue("@inRemake", stremake);
                            }
                            SqlParameter returnValue = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                            //  returnValue.Direction = System.Data.ParameterDirection.Output;
                            //  command.Parameters.Add(returnValue); 
                            returnValue.Direction = System.Data.ParameterDirection.Output;
                            command.Parameters.Add(returnValue);


                            command.ExecuteNonQuery();

                            messagereturnsql = returnValue.Value.ToString();
                            //S20161016
                            command.Dispose();
                            //E20161016
                            //if (messagereturnsql == "N")
                            //{
                               

                            //}



                        }
                        connection.Close();

                    }
                }
                message = "true";
            }
            catch (Exception ex)
            {
                message = "false";
                messageerror = ex.Message;

            }
            return Json(new { message, messageerror }, JsonRequestBehavior.AllowGet);
            //return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Cancalpriceapproval(string CartID, string User)
        {
            int id = 0;
            bool message = false;
            try
            {
                //id = Convert.ToInt32(CartID);
                //Ordering_Cart Update = db.Ordering_Cart.First(c => c.ID == id);
                //Update.Status = "C";

                //Update.Updated_Date = DateTime.Now;
                //Update.Updated_By = User;
                //db.SaveChanges();

                message = true;
            }
            catch (Exception ex)
            {
                message = false;
            }
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
    }
    public class STKGRPList
    {
        public string STKGRP { get; set; }
        public string GRPNAM { get; set; }
    }

    public class Itemapproval
    {
        public string cartid { get; set; }
        public string PRCLST_NO { get; set; }
        public string spcend_date { get; set; }
        public string spcmoq { get; set; }
        public string remake { get; set; }
        public string saleprice { get; set; }
    }
    public class ItemList_PriceApprove
    {
        public ItemPriceApprove val { get; set; }

    }
    public class ItemPriceApprove
    {
        public string SONumber { get; set; }
        public string SODate { get; set; }
        public string RowNo { get; set; }
        public string ID { get; set; }
        public string PRCLST_NO { get; set; }
        public string CUSCOD { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string STKGRP { get; set; }
        public string MINORD { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public string spcmoq { get; set; }
        public string SpecialPrice { get; set; }
        public string spcstart_date { get; set; }
        public string spcend_date { get; set; }
        public string ExpectPrice { get; set; }
        public string Qty { get; set; }
        public string User { get; set; }
        public string Amt { get; set; }
        public string ORDDAT { get; set; }
        public string LineNote { get; set; }
        public string Status { get; set; }
        public string StatusFull { get; set; }
        public string Discount { get; set; }
        public int QtyAmt { get; set; }
        public string AmtQty { get; set; }
        public string AmtSalePrices { get; set; }
        public string TotalAmt { get; set; }
        public string amtCredit { get; set; }
        public string AmtDiscount { get; set; }
        public string UOM { get; set; }
        public string SLMID { get; set; }
        public string PromotionDesc { get; set; }
        public string Promotion { get; set; }
        public string LastInvdate { get; set; }
        public string LastInvPrice { get; set; }
        public string PrcApproveDate { get; set; }
        public string InsertedDate { get; set; }
        public string InsertedBy { get; set; }

        public string Prcdes { get; set; }
        public string UNITCOST { get; set; }
        public string readyQty { get; set; }

    }
}
