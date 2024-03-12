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

namespace NewShop.Models
{
    public class DataCenterController : Controller
    {
        //
        // GET: /DataCenter/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Getdiscount(string cuscod)
        {
            string discount = string.Empty;
            string Note = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_CusDiscount", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@cuscode", cuscod);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                discount = dr["Discount"].ToString();
                Note = dr["Note"].ToString();
            }
            dr.Dispose();
            command.Dispose();
            Connection.Dispose();
            Connection.Close();
            return Json(new { discount, Note }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdate(string Name)
        {
            List<LookupVehicle> List = new List<LookupVehicle>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_LookupVehicle", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Type", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new LookupVehicle()
                {
                    //Type = dr["Type"].ToString(),
                    Code = dr["Maker"].ToString(),
                    //Description = dr["Description"].ToString(),
                    //SearchDescription = dr["Search Description"].ToString(),
                    //CodeRelation = dr["Code Relation"].ToString(),
                    //YrStart = dr["Yr Start"].ToString(),
                    //YrEnd = dr["Yr End"].ToString(),
                    //EngineType = dr["Engine Type"].ToString(),
                    //CC = dr["CC"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdatalogincustomer()
        {
            List<CUS> List = new List<CUS>();
            string usre = Session["UserID"].ToString();
            string password = Session["UserPassword"].ToString();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Chk_Customer", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UsrID", usre);
            command.Parameters.AddWithValue("@Password", password);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new CUS()
                {
                    CUSCOD = dr["CUSCOD"].ToString(),
                    CUSNAM = dr["CUSNAM"].ToString(),
                    PRO = dr["PRO"].ToString(),
                    ADDR_01 = dr["ADDR_01"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCatProductGroup(string com, string cuscod)
        {
            List<CatProductGroup> List = new List<CatProductGroup>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_CatProductGroup", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Com", com);
            command.Parameters.AddWithValue("@inCUSCOD", cuscod);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new CatProductGroup()
                {
                    Company = dr["Company"].ToString(),
                    ProductGroup = dr["Product Group"].ToString(),


                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCatProductGroupline(string com, string progroup, string brand, string cuscod)
        {
            List<CatProductGroup> List = new List<CatProductGroup>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_CatProductLine", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Com", com);
            command.Parameters.AddWithValue("@progroup", progroup);
            command.Parameters.AddWithValue("@Brand", brand);
            command.Parameters.AddWithValue("@inCUSCOD", cuscod);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new CatProductGroup()
                {
                    Company = dr["Company"].ToString(),
                    ProductGroup = dr["Product Group"].ToString(),
                    ProductLine = dr["Product Line"].ToString()

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateRelation(string Name, string sty)
        {
            List<LookupVehicle> List = new List<LookupVehicle>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            //var command = new SqlCommand("P_Search_LookupVehicleRelation", Connection);
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.AddWithValue("@Sty", sty);
            //command.Parameters.AddWithValue("@Type", Name);
            var command = new SqlCommand("P_Search_LookupVehicle", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Type", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new LookupVehicle()
                {
                    // Type = dr["Type"].ToString(),
                    Code = dr["Model"].ToString(),
                    //Description = dr["Search Description"].ToString(),
                    // SearchDescription = dr["Search Description"].ToString(),
                    // CodeRelation = dr["Code Relation"].ToString(),
                    //YrStart = dr["Yr Start"].ToString(),
                    //YrEnd = dr["Yr End"].ToString(),
                    //EngineType = dr["Engine Type"].ToString(),
                    //CC = dr["CC"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBrand(string Name, string productLine)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            List<Brabdgrop> List = new List<Brabdgrop>();
            SqlCommand cmd = new SqlCommand("P_Search_Brand", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Name);
            cmd.Parameters.AddWithValue("@ProductLine", productLine);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new Brabdgrop()
                {
                    CODE = dr["Brand"].ToString(),

                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSegment(string Name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            List<Segmentgrop> List = new List<Segmentgrop>();
            SqlCommand cmd = new SqlCommand("P_Search_Segment", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            // cmd.Parameters.AddWithValue("@Company", Name);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new Segmentgrop()
                {
                    CODE = dr["code"].ToString(),
                    segment = dr["segment"].ToString(),
                    sort = dr["sort"].ToString(),

                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateStkgrp(string Name)
        {
            List<Stkgrop> List = new List<Stkgrop>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Mst_StkGrp", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Com", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new Stkgrop()
                {
                    STKGRP = dr["STKGRP"].ToString(),
                    GRPNAM = dr["GRPNAM"].ToString(),
                    SEC = dr["SEC"].ToString(),
                    PROD = dr["PROD"].ToString(),
                    DEP = dr["DEP"].ToString(),
                    COMPANY = dr["COMPANY"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathImagemain(string inCLM_ID, string CLM_NO)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            // var root = @"\Warranty\ImgUpload\";
            var root = @"..\IMAGE_A\";
            var command = new SqlCommand("P_GetPathImage", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@incom", inCLM_ID);
            command.Parameters.AddWithValue("@instk", CLM_NO);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                //model = new ImageFiles();
                //model.IMAGE_ID = dr["IMAGE_ID"].ToString();
                //model.REQ_NO = dr["REQ_NO"].ToString();
                //model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                //model.IMAGE_NO = dr["IMAGE_NO"].ToString();
                //model.IMAGE_NAME = dr["IMAGE_NAME"].ToString();
                //model.PATH = dr["PATH"].ToString();
                //  model.PATH = Server.MapPath(@"~\ImgUpload\" + dr["IMAGE_NAME"].ToString());
                //model.PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString());
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                // Getdata.Add(new ImageFilesListDetail { val = model });
            }
            //dr.Close();
            //dr.Dispose();
            //command.Dispose();
            //Connection.Close();
            //return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
            return null;

        }
        public JsonResult Getdateslm()
        {
            string usre = Session["UserID"].ToString();
            List<SLM> SlmList = new List<SLM>();

            SLM SlmListcount = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Chk_user", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UsrID", usre);
            command.Parameters.AddWithValue("@Password", "");

            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                SlmList.Add(new SLM()
                {
                    SLMCOD = dr["SLMCOD"].ToString(),
                    SLMNAM = dr["SLMNAM"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();

            //transaction.Commit();
            return Json(SlmList, JsonRequestBehavior.AllowGet);


        }
        public JsonResult Getdateslmbycustomer(string cuscod)
        {


            List<SLM> SlmList = new List<SLM>();

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Search_SLM_byCustomer", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@incode", cuscod);


            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                SlmList.Add(new SLM()
                {
                    SLMCOD = dr["SLMCOD"].ToString(),
                    SLMNAM = dr["SLMNAM"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();

            //transaction.Commit();
            return Json(SlmList, JsonRequestBehavior.AllowGet);


        }
        public JsonResult Getdateslmbysalmcod(string codeslm)
        {

            List<SLM> SlmList = new List<SLM>();
            SLM SlmListcount = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Search_SLM", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@incode", codeslm);


            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                SlmList.Add(new SLM()
                {
                    SLMCOD = dr["SLMCOD"].ToString(),
                    SLMNAM = dr["SLMNAM"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();

            //transaction.Commit();
            return Json(SlmList, JsonRequestBehavior.AllowGet);


        }
        public JsonResult Getdatabyslm(string SLXX, string SLMNAM)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            SqlCommand cmd = new SqlCommand("select * from v_CUSPROV where SLMCOD =N'" + SLXX + "' order by SLMCOD", Connection);
            this.Session["SLM"] = SLXX;
            this.Session["SLMCOD"] = SLMNAM;
            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                CUSList.Add(new CUS()
                {
                    CUSCOD = rev_CUSPROV["CUSCOD"].ToString(),
                    CUSNAM = rev_CUSPROV["CUSNAM"].ToString(),
                    PRO = rev_CUSPROV["PRO"].ToString(),
                    ADDR_01 = rev_CUSPROV["ADDR_01"].ToString(),
                    CUSTYP = rev_CUSPROV["CUSTYP"].ToString(),
                    AACCrlimit = rev_CUSPROV["AACCRLINE"].ToString(),
                    AACBalance = rev_CUSPROV["AACBAL"].ToString(),
                    TACCrlimit = rev_CUSPROV["TACCRLINE"].ToString(),
                    TACBalance = rev_CUSPROV["TACBAL"].ToString()
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
        public JsonResult GetdCustomerCredit(string cusel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();

            var cmd = new SqlCommand("P_Customer_credit", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CUSCOD", cusel);


            string cusstr = string.Empty;
            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                CUSList.Add(new CUS()
                {
                    CUSCOD = rev_CUSPROV["CUSCOD"].ToString(),
                    CUSNAM = rev_CUSPROV["CUSNAM"].ToString(),
                    PRO = rev_CUSPROV["PRO"].ToString(),
                    ADDR_01 = rev_CUSPROV["ADDR_01"].ToString(),
                    ADDR_02 = rev_CUSPROV["ADDR_02"].ToString(),
                    CUSTYP = rev_CUSPROV["CUSTYP"].ToString(),
                    AACCrlimit = rev_CUSPROV["AACCRLINE"].ToString(),
                    AACBalance = rev_CUSPROV["AACBAL"].ToString(),
                    TACCrlimit = rev_CUSPROV["TACCRLINE"].ToString(),
                    TACBalance = rev_CUSPROV["TACBAL"].ToString(),
                    SLMCOD = rev_CUSPROV["SLMCOD"].ToString(),
                    INACTIVE = rev_CUSPROV["INACTIVE"].ToString(),
                    BLOCKED = rev_CUSPROV["BLOCKED"].ToString(),
                    AACPAYTRM = rev_CUSPROV["AACPAYTRM"].ToString(),
                    TACPAYTRM = rev_CUSPROV["TACPAYTRM"].ToString(),
                    TELNUM = rev_CUSPROV["TELNUM"].ToString(),
                });
            }
            //this.Session["CUSCOD"] = CUSList[0].CUSCOD;
            //rev_CUSPROV.Dispose();
            //S20161016
            rev_CUSPROV.Close();
            rev_CUSPROV.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(CUSList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdateCus(string Cus, string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string query = string.Empty;
            if (Slm == "0" || Slm == null || Slm == "(ALL)")
            {

                SLMCOD = "";
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM from v_CUSPROV pc    where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", Cus);
            }
            else
            {
                SLMCOD = Slm;
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM from v_CUSPROV pc    where  pc.SLMCOD ='" + SLMCOD + "' and (pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%')", Cus);

            }

            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016
            }


            Connection.Close();

            return Json(Code, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdatabyCus(string cusel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            SqlCommand cmd = new SqlCommand("select * from v_CUSPROV where CUSCOD =N'" + cusel + "' order by SLMCOD", Connection);

            string cusstr = string.Empty;
            SqlDataReader rev_CUSPROV = cmd.ExecuteReader();
            while (rev_CUSPROV.Read())
            {
                CUSList.Add(new CUS()
                {
                    CUSCOD = rev_CUSPROV["CUSCOD"].ToString(),
                    CUSNAM = rev_CUSPROV["CUSNAM"].ToString(),
                    PRO = rev_CUSPROV["PRO"].ToString(),
                    ADDR_01 = rev_CUSPROV["ADDR_01"].ToString(),
                    ADDR_02 = rev_CUSPROV["ADDR_02"].ToString(),
                    CUSTYP = rev_CUSPROV["CUSTYP"].ToString(),
                    AACCrlimit = rev_CUSPROV["AACCRLINE"].ToString(),
                    AACBalance = rev_CUSPROV["AACBAL"].ToString(),
                    TACCrlimit = rev_CUSPROV["TACCRLINE"].ToString(),
                    TACBalance = rev_CUSPROV["TACBAL"].ToString(),
                    SLMCOD = rev_CUSPROV["SLMCOD"].ToString(),
                    INACTIVE = rev_CUSPROV["INACTIVE"].ToString(),
                    BLOCKED = rev_CUSPROV["BLOCKED"].ToString(),
                    AACPAYTRM = rev_CUSPROV["AACPAYTRM"].ToString(),
                    TACPAYTRM = rev_CUSPROV["TACPAYTRM"].ToString(),
                    TELNUM = rev_CUSPROV["TELNUM"].ToString(),
                    RATING = rev_CUSPROV["Rating"].ToString(),
                    Hierarchy1_Market_Segment = rev_CUSPROV["Hierarchy1 (Market Segment)"].ToString(),
                    Hierarchy2_Channel = rev_CUSPROV["Hierarchy2 (Channel)"].ToString(),
                    Hierarchy3_Bussiness_Type = rev_CUSPROV["Hierarchy3 (Bussiness Type)"].ToString(),
                });
            }
            //this.Session["CUSCOD"] = CUSList[0].CUSCOD;
            //rev_CUSPROV.Dispose();
            //S20161016
            rev_CUSPROV.Close();
            rev_CUSPROV.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(CUSList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdateCusCode(string Name, string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<CUS> CUSList = new List<CUS>();
            string SLMCOD = string.Empty;
            string query = string.Empty;
            if (Slm == "ALL")
            {

                SLMCOD = "";
                //query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  from v_CUSPROV pc    where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", Name);
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM + '----ที่อยู่ ' +  pc.ADDR_01+ ' | ' + pc.PRO from v_CUSPROV pc    where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", Name);
            }
            else
            {
                SLMCOD = Slm;
                // query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  from v_CUSPROV pc    where  pc.SLMCOD ='" + SLMCOD + "' and (pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%')", Name);
                query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM+ ' ----ที่อยู่' +  pc.ADDR_01+ ' | ' + pc.PRO from v_CUSPROV pc    where  pc.SLMCOD ='" + SLMCOD + "' and (pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%')", Name);
            }

            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Code.Add(reader.GetString(1));
                }
                // reader.Dispose();
                //S20161016
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                //E20161016
                Connection.Close();
            }


            // Connection.Close();

            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataCuslogincus(string cusel)
        {
            List<logincutomer> List = new List<logincutomer>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_CusByCustomer", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCUS", cusel);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new logincutomer()
                {
                    EmpID = dr["EmpID"].ToString(),
                    company = dr["company"].ToString(),
                    UsrID = dr["UsrID"].ToString(),
                    initials = dr["initials"].ToString(),
                    Department = dr["Department"].ToString(),
                    Position = dr["Position"].ToString(),
                    EMail = dr["EMail"].ToString(),
                    CUSCOD = dr["CUSCOD"].ToString(),
                    SUP = dr["SUP"].ToString(),
                    UsrTyp = dr["UsrTyp"].ToString(),
                    SLMCOD = dr["SLMCOD"].ToString(),
                    SLMNAM = dr["SLMNAM"].ToString(),
                    PasswordExpiredDate = dr["Password Expired Date"].ToString(),
                    DatetoExpire = dr["Date to Expire"].ToString(),
                    SLMPhone = dr["SLMPhone"].ToString(),
                    SalesCo = dr["SalesCo"].ToString(),
                    SalesCoPhone = dr["SalesCoPhone"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataCompanyInformation(string Slm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            var Getdata = new List<object>();

            SqlCommand cmd = new SqlCommand("select *    from dbo.v_SLMTAB  where [SLMCOD] ='" + Slm + "' order by [SLMCOD] ", Connection);
            SqlDataReader rev_Mod = cmd.ExecuteReader();

            while (rev_Mod.Read())
            {
                Getdata.Add(new
                {
                    SLMCOD = rev_Mod["SLMCOD"].ToString(),
                    SLMNAM = rev_Mod["SLMNAM"].ToString(),
                    Phone = rev_Mod["Phone"].ToString(),
                    SalesCo = rev_Mod["SalesCo"].ToString(),
                    SalesCoPhone = rev_Mod["SalesCoPhone"].ToString(),


                });
            }
            //S20161016
            rev_Mod.Close();
            rev_Mod.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataCompanyInfo()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            var Getdata = new List<object>();

            SqlCommand cmd = new SqlCommand("select *    from dbo.Company ", Connection);
            SqlDataReader rev_Mod = cmd.ExecuteReader();

            while (rev_Mod.Read())
            {
                Getdata.Add(new
                {
                    Company = rev_Mod["Company"].ToString(),
                    Name = rev_Mod["Name"].ToString(),
                    Address1 = rev_Mod["Address1"].ToString(),
                    Address2 = rev_Mod["Address2"].ToString(),
                    Phone = rev_Mod["Phone"].ToString(),
                    Fax = rev_Mod["Fax"].ToString(),
                    Tax = rev_Mod["Tax"].ToString(),
                    Bank1 = rev_Mod["Bank1"].ToString(),
                    Bank2 = rev_Mod["Bank2"].ToString(),

                });
            }
            //S20161016
            rev_Mod.Close();
            rev_Mod.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdateContorder(string Slm, string cus, string Usertype, string UserIn)
        {
            //Add Function Count  Wait Confirm  order 04/04/2017//
            List<Listslmcount> Getdata = new List<Listslmcount>();
            SLMc SlmListcount = null;
            string SLMc = string.Empty;
            string Cusc = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();


            var commandCount_Orde = new SqlCommand("P_Count_Order_By_Sales_catalog", Connection);

            commandCount_Orde.CommandType = CommandType.StoredProcedure;
            commandCount_Orde.Parameters.AddWithValue("@Salecode", Slm);
            commandCount_Orde.Parameters.AddWithValue("@Cuscode", cus);
            commandCount_Orde.Parameters.AddWithValue("@Usertype", Usertype);
            commandCount_Orde.Parameters.AddWithValue("@UserIn", UserIn);
            //commandCount_Orde.ExecuteNonQuery();
            SqlDataReader drCount_Orde = commandCount_Orde.ExecuteReader();

            while (drCount_Orde.Read())
            {

                SlmListcount = new SLMc();

                SlmListcount.SumQty = drCount_Orde["SumQty"].ToString();
                SlmListcount.Countrow = drCount_Orde["Countrow"].ToString();
                SlmListcount.CountPN = drCount_Orde["CountPN"].ToString();
                SlmListcount.Status = drCount_Orde["Status"].ToString();
                Getdata.Add(new Listslmcount { val = SlmListcount });

            }

            drCount_Orde.Dispose();
            commandCount_Orde.Dispose();


            Connection.Dispose();

            Connection.Close();

            //transaction.Commit();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataPlusItem(string Nodisplay, string strcustome)
        {
            string com = string.Empty;
            string substkgrp = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            // List<SearchitemDetailGetdata> Getdata = new List<SearchitemDetailGetdata>();
            //Searchitem model = null;
            var Getdata = new List<object>();
            //var Getdata = new List<object>();
            try
            {

                var root = @"..\IMAGE_A\";
                var command = new SqlCommand("p_Search_Item_Byvehicle_PlusItem", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pCusCod", strcustome);
                command.Parameters.AddWithValue("@pStkcod", Nodisplay);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        Company = dr["Company"].ToString(),
                        STKCOD = dr["STKCOD"].ToString(),
                        Description = dr["Description"].ToString(),
                        //Stock = dr["Stock"].ToString(),
                        EndPrice = dr["End Price"].ToString(),
                        PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString())
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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataContinue_Newitem(string strcustome)
        {
            string com = string.Empty;
            string substkgrp = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            // List<SearchitemDetailGetdata> Getdata = new List<SearchitemDetailGetdata>();
            //Searchitem model = null;
            var Getdata = new List<object>();
            //var Getdata = new List<object>();
            try
            {

                var root = @"..\IMAGE_A\";
                var command = new SqlCommand("p_Search_NewItem", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pCusCod", strcustome);
                //command.Parameters.AddWithValue("@pStkcod", Nodisplay);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        Company = dr["Company"].ToString(),
                        STKCOD = dr["STKCOD"].ToString(),
                        Description = dr["STKDES"].ToString(),
                        //Stock = dr["Stock"].ToString(),
                        EndPrice = dr["End Price"].ToString(),
                        PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString())
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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataContinue_Stkgrp(string Nodisplay, string strcustome)
        {
            string com = string.Empty;
            string substkgrp = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            // List<SearchitemDetailGetdata> Getdata = new List<SearchitemDetailGetdata>();
            //Searchitem model = null;
            var Getdata = new List<object>();
            //var Getdata = new List<object>();
            try
            {

                var root = @"..\IMAGE_A\";
                var command = new SqlCommand("p_Search_Item_Continue_Stkgrp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pCusCod", strcustome);
                command.Parameters.AddWithValue("@pStkcod", Nodisplay);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        Company = dr["Company"].ToString(),
                        STKCOD = dr["STKCOD"].ToString(),
                        Description = dr["Description"].ToString(),
                        Stock = dr["Stock"].ToString(),
                        EndPrice = dr["End Price"].ToString(),
                        PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString())
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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Getdatailorder(string codval)
        {
            List<ListsDetailSLM> Getdata = new List<ListsDetailSLM>();
            DetailSLM DetailLists = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_detail_Order_By_Sales_catalog", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Salecode", codval);


            SqlDataReader dr_Orde = command.ExecuteReader();

            while (dr_Orde.Read())
            {

                DetailLists = new DetailSLM();

                DetailLists.salmman = dr_Orde["SLMCODE"].ToString();
                DetailLists.salmmanname = dr_Orde["SLMNAM"].ToString();
                DetailLists.customer = dr_Orde["CUSCOD"].ToString();
                DetailLists.customername = dr_Orde["CUSNAM"].ToString();
                DetailLists.Countrow = dr_Orde["Countrow"].ToString();
                DetailLists.sumqty = dr_Orde["SumQty"].ToString();
                Getdata.Add(new ListsDetailSLM { val = DetailLists });

            }

            dr_Orde.Dispose();
            command.Dispose();


            Connection.Dispose();

            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdatailorderbycustomer(string codval)
        {
            List<ListsDetailSLM> Getdata = new List<ListsDetailSLM>();
            DetailSLM DetailLists = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_detail_Order_By_Cus_catalog", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Cuscod", codval);


            SqlDataReader dr_Orde = command.ExecuteReader();

            while (dr_Orde.Read())
            {

                DetailLists = new DetailSLM();

                DetailLists.salmman = dr_Orde["SLMCODE"].ToString();
                DetailLists.salmmanname = dr_Orde["SLMNAM"].ToString();
                DetailLists.customer = dr_Orde["CUSCOD"].ToString();
                DetailLists.customername = dr_Orde["CUSNAM"].ToString();
                DetailLists.Countrow = dr_Orde["Countrow"].ToString();
                DetailLists.sumqty = dr_Orde["SumQty"].ToString();
                Getdata.Add(new ListsDetailSLM { val = DetailLists });

            }

            dr_Orde.Dispose();
            command.Dispose();


            Connection.Dispose();

            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataCusShipping(string XXcus)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            List<ItemListshipto> cusshipping = new List<ItemListshipto>();
            shipto model = null;
            // SqlCommand cmd = new SqlCommand("select *    from dbo.v_NVcust_ShiptoAddr  where [customer No_] ='"+XXcus+"' and code not like 'N9%' ", Connection);
            SqlCommand cmd = new SqlCommand("select *    from dbo.v_NVcust_ShiptoAddr  where [customer No_] ='" + XXcus + "' order by [Code] ", Connection);
            SqlDataReader rev_Mod = cmd.ExecuteReader();
            //var cusshipping = db.v_NVcust_ShiptoAddr.Where(c => c.Customer_No_ == XXcus && c.Code != "N9" || c.Code != "N99").ToArray();
            // var cusshipping = rev_Mod;
            // ID = rev_CUSTYP.GetValue(0).ToString(),
            while (rev_Mod.Read())
            {
                model = new shipto();
                model.customer = rev_Mod["Customer No_"].ToString();
                model.code = rev_Mod["Code"].ToString();
                model.name = rev_Mod["Name"].ToString();
                model.name2 = rev_Mod["Name 2"].ToString();
                model.address = rev_Mod["Address"].ToString();
                model.address2 = rev_Mod["Address 2"].ToString();
                model.city = rev_Mod["City"].ToString();
                model.postcode = rev_Mod["Post code"].ToString();
                cusshipping.Add(new ItemListshipto { val = model });
            }
            //S20161016
            rev_Mod.Close();
            rev_Mod.Dispose();
            cmd.Dispose();
            //E20161016
            Connection.Close();
            return Json(cusshipping, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdateStockCodedropdownlist(string Prod, string STKGR, string Xcus, string XvalCompany, string Xval, string names)
        {
            string CUSCOD = string.Empty;
            List<ItemListdropList> StockCode = new List<ItemListdropList>();
            ItemListdrop DetailLists = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Item_dropdownlist", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inProd", Prod);
            command.Parameters.AddWithValue("@inSTKGRP", STKGR);
            command.Parameters.AddWithValue("@inFix", Xval);
            command.Parameters.AddWithValue("@Company", XvalCompany);
            command.Parameters.AddWithValue("@inName", names);
            Connection.Open();
            //command.ExecuteNonQuery();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {


                DetailLists = new ItemListdrop();

                DetailLists.No = dr["STKCOD"].ToString();
                DetailLists.STKDES = dr["STKDES"].ToString();

                StockCode.Add(new ItemListdropList { val = DetailLists });

            }
            //dr.Close();
            //S20161016
            dr.Close();
            dr.Dispose();
            command.Dispose();
            //E20161016

            Connection.Close();

            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProd()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            List<Prod> List = new List<Prod>();
            SqlCommand cmd = new SqlCommand("P_Search_PROD", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            // cmd.Parameters.AddWithValue("@Company", Name);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new Prod()
                {
                    CODE = dr["PROD"].ToString(),
                    NAME = dr["PRODNAME"].ToString()


                });

            }
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetdateStkgrpByProd(string Name)
        {
            List<Stkgrop> List = new List<Stkgrop>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Mst_StkGrp_ByPROD", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PROD", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new Stkgrop()
                {
                    STKGRP = dr["STKGRP"].ToString(),
                    GRPNAM = dr["GRPNAM"].ToString(),
                    SEC = dr["SEC"].ToString(),
                    PROD = dr["PROD"].ToString(),
                    DEP = dr["DEP"].ToString(),
                    COMPANY = dr["COMPANY"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetdataCusAmt(string strcustome)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();

            try
            {


                var command = new SqlCommand("p_Search_CusAmt", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", strcustome);

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        Cuscod = dr["Cuscod"].ToString(),
                        Company = dr["Company"].ToString(),
                        AmtYTD = dr["Amt YTD"].ToString(),
                        AmtMTD = dr["Amt MTD"].ToString(),
                        Jan = dr["Jan"].ToString(),
                        Feb = dr["Feb"].ToString(),
                        Mar = dr["Mar"].ToString(),
                        Apr = dr["Apr"].ToString(),
                        May = dr["May"].ToString(),
                        Jun = dr["Jun"].ToString(),
                        Jul = dr["Jul"].ToString(),
                        Aug = dr["Aug"].ToString(),
                        Sep = dr["Sep"].ToString(),
                        Nov = dr["Nov"].ToString(),
                        Dec = dr["Dec"].ToString(),


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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataCusAmtchild(string strcustome)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();

            try
            {


                var command = new SqlCommand("p_Search_CusAmt_child", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", strcustome);

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        Cuscod = dr["Cuscod"].ToString(),
                        Company = dr["Company"].ToString(),
                        AmtYTD = dr["Amt YTD"].ToString(),
                        AmtMTD = dr["Amt MTD"].ToString(),
                        Jan = dr["Jan"].ToString(),
                        Feb = dr["Feb"].ToString(),
                        Mar = dr["Mar"].ToString(),
                        Apr = dr["Apr"].ToString(),
                        May = dr["May"].ToString(),
                        Jun = dr["Jun"].ToString(),
                        Jul = dr["Jul"].ToString(),
                        Aug = dr["Aug"].ToString(),
                        Sep = dr["Sep"].ToString(),
                        Nov = dr["Nov"].ToString(),
                        Dec = dr["Dec"].ToString(),


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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataCusAmtTop20(string cuscod)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();
            try
            {
                var command = new SqlCommand("p_Search_CusItmGrp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", cuscod);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Getdata.Add(new
                    {
                        Rowid = dr["Rowid"].ToString(),
                        Company = dr["Company"].ToString(),
                        Cuscod = dr["CUSCOD"].ToString(),
                        Stkcod = dr["STKCOD"].ToString(),
                        Stkdes = dr["STKDES"].ToString(),
                        Qty = dr["Qty"].ToString(),
                        Amt = dr["AMT"].ToString()
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
            return Json(Getdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataPromotion_Cus(string strcustome, string period)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();

            try
            {


                var command = new SqlCommand("P_Search_Promotion_Cus", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", strcustome);
                command.Parameters.AddWithValue("@Period", period);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        CUSCOD = dr["CUSCOD"].ToString(),
                        company = dr["company"].ToString(),
                        Promotion_Name = dr["Promotion_Name"].ToString(),
                        StartDate = dr["StartDate"].ToString(),
                        EndDate = dr["EndDate"].ToString(),
                        //Con_Description = dr["Condition"].ToString(),
                        Condition = dr["Condition"].ToString(),
                        // UOM = dr["UOM"].ToString(),
                        INVAMT = dr["Invoice Amount"].ToString(),
                        PaidAmt = dr["Invoice Paid"].ToString(),
                        Reward = dr["Reward"].ToString(),
                        RemainAmt = dr["Remaining Amount"].ToString(),



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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataWarrantyClaim_Cus_count(string strcustome)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();

            try
            {


                var command = new SqlCommand("P_Search_WarrantyClaim_Cus_count", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", strcustome);

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        A = dr["รอดำเนินงาน"].ToString(),
                        B = dr["กำลังตรวจสอบ"].ToString(),
                        C = dr["รอส่งสินค้าทดแทน"].ToString(),

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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdataWarrantyClaim_Cus(string strcustome, string tap)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string message = "false";
            var Getdata = new List<object>();

            try
            {


                var command = new SqlCommand("P_Search_WarrantyClaim_Cus", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCUSCOD", strcustome);
                command.Parameters.AddWithValue("@instatus", tap);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    Getdata.Add(new
                    {
                        REQ_NO = dr["REQ_NO"].ToString(),
                        CLM_NO_SUB = dr["CLM_NO_SUB"].ToString(),
                        REQ_DATE = dr["REQ_DATE"].ToString(),
                        ReceiveDate = dr["FormatReceiveDate"].ToString(),
                        CLM_COMPANY = dr["CLM_COMPANY"].ToString(),
                        CUSCOD = dr["CUSCOD"].ToString(),
                        STKCOD = dr["STKCOD"].ToString(),
                        STKDES = dr["STKDES"].ToString(),
                        Qty = dr["Qty"].ToString(),
                        InvoiceNo = dr["Invoice No"].ToString(),
                        InvoiceDate = dr["Invoice Date"].ToString(),
                        Symptom = dr["Symptom"].ToString(),
                        Request = dr["Request"].ToString(),
                        DueDate = dr["Due Date"].ToString(),
                        Checking = dr["Checking"].ToString(),
                        ApproveDate = dr["Approve Date"].ToString(),
                        Status = dr["Status"].ToString(),




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

            return Json(Getdata, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetdataSessionlogin(string UsrID, string SessionId)
        {
            string StrStstuslogin = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("P_Update_SessionId_Customer", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsrID", UsrID);
                cmd.Parameters.AddWithValue("@SessionId", SessionId);
                SqlParameter returnValue = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);

                returnValue.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                cmd.ExecuteReader();
                StrStstuslogin = returnValue.Value.ToString();

                cmd.Dispose();

                conn.Close();
                //}
            }
            catch (Exception ex)
            {
                message = ex.Message + '/' + ex.Source + '/' + ex.HelpLink + '/' + ex.HResult;
                //return -1;
            }


            return Json(new { message, StrStstuslogin }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetdataPrivilege(string UsrID, string cuscod)
        {
            string StrStstuslogin = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("P_privilege_Customer", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsrID", UsrID);
                cmd.Parameters.AddWithValue("@cuscod", cuscod);
                SqlParameter returnValue = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);

                returnValue.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                cmd.ExecuteReader();
                StrStstuslogin = returnValue.Value.ToString();

                cmd.Dispose();

                conn.Close();
                //}
            }
            catch (Exception ex)
            {
                message = ex.Message + '/' + ex.Source + '/' + ex.HelpLink + '/' + ex.HResult;
                //return -1;
            }


            return Json(new { message, StrStstuslogin }, JsonRequestBehavior.AllowGet);

        }
    }
}



