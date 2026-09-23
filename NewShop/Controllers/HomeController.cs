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
using NewShop.Helpers;
using System.Globalization;

namespace NewShop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //this.Session["UserType"] = "";
            if (this.Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");

            }

            return View();
        }
        public JsonResult GetdateStockCode(string Name, string Xval, string Prod, string STKGR, string Xcus, string XvalCompany)
        {
            string CUSCOD = string.Empty;
            List<string> StockCode = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Item_Catalog", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inCUSCOD", Xcus);
            command.Parameters.AddWithValue("@inSearch", Name);
            command.Parameters.AddWithValue("@inProd", "");
            command.Parameters.AddWithValue("@inSTKGRP", "");
            command.Parameters.AddWithValue("@inFix ", Xval);
            command.Parameters.AddWithValue("@Company", XvalCompany);
            Connection.Open();
            //command.ExecuteNonQuery();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                //StockCode.Add(reader.GetString(0) + "|" + reader.GetString(1));
                StockCode.Add(dr.GetString(1) + "|" + dr.GetString(2));
            }
            //S20161016                
            dr.Dispose();
            command.Dispose();
            //E20161016
            Connection.Dispose();
            Connection.Close();
            //}
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateStockCodebyval(string usrtyp, string itemsegment, string engine, string cuscod, string yrStart, string yrEnd, string itemno, string maker, string modelno, string submodel, string company, string catalogue, string brand, string textfree)
        {
            string CUSCOD = string.Empty;
            List<string> StockCode = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            var command = new SqlCommand("p_Search_item_byVehicle", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Maker", maker);
            command.Parameters.AddWithValue("@Model", modelno);
            command.Parameters.AddWithValue("@SubModel", submodel);
            command.Parameters.AddWithValue("@YrStart", yrStart);
            command.Parameters.AddWithValue("@YrEnd", yrEnd);
            command.Parameters.AddWithValue("@Engine", engine);
            command.Parameters.AddWithValue("@Category", catalogue);
            command.Parameters.AddWithValue("@Item", itemno);
            command.Parameters.AddWithValue("@Brand", brand);
            command.Parameters.AddWithValue("@inCUSCOD", cuscod);
            command.Parameters.AddWithValue("@inSearch", textfree);
            command.Parameters.AddWithValue("@Company", company);
            command.Parameters.AddWithValue("@ItemSegment", itemsegment);
            command.Parameters.AddWithValue("@UsrTyp", usrtyp);
            //command.ExecuteNonQuery();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                //StockCode.Add(reader.GetString(0) + "|" + reader.GetString(1));
                StockCode.Add(dr.GetString(1));
            }
            //S20161016                
            dr.Dispose();
            command.Dispose();
            //E20161016
            Connection.Dispose();
            Connection.Close();
            //}
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public JsonResult Getdataitemdetail(string usrtyp, string sortby, string itemsegment, string productgroup, string productline, string engine, string cuscod, string yrStart, string yrEnd, string itemno, string maker, string modelno, string submodel, string company, string catalogue, string brand, string textfree, string usrId) {
            if (!User.Identity.IsAuthenticated || Session["UserType"] == null) {
                Response.StatusCode = 401;
                return Json(new { error = "Unauthorized - กรุณา login ก่อนใช้งาน" });
            }

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            string message = "false";
            var root = @"..\IMAGE_A\";
            var Getdata = new List<object>();
            long signatureTimestamp = PriceSignatureHelper.GetCurrentTimestamp();

            SqlCommand command = null;
            SqlDataReader dr = null;

            try {
                Connection.Open();

                command = new SqlCommand("p_Search_item_byVehicle", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Maker", maker);
                command.Parameters.AddWithValue("@Model", modelno);
                command.Parameters.AddWithValue("@SubModel", submodel);
                command.Parameters.AddWithValue("@YrStart", yrStart);
                command.Parameters.AddWithValue("@YrEnd", yrEnd);
                command.Parameters.AddWithValue("@Engine", engine);
                command.Parameters.AddWithValue("@Category", catalogue);
                command.Parameters.AddWithValue("@OE", itemno);
                command.Parameters.AddWithValue("@Brand", brand);
                command.Parameters.AddWithValue("@inCUSCOD", cuscod);
                command.Parameters.AddWithValue("@inSearch", textfree);
                command.Parameters.AddWithValue("@Company", company);
                command.Parameters.AddWithValue("@PrdGrp", productgroup);
                command.Parameters.AddWithValue("@PrdLne", productline);
                command.Parameters.AddWithValue("@ItemSegment", itemsegment);
                command.Parameters.AddWithValue("@SortBy", sortby);
                command.Parameters.AddWithValue("@UsrTyp", usrtyp);
                command.Parameters.AddWithValue("@UsrID", usrId);
                dr = command.ExecuteReader();

                while (dr.Read()) {
                    string stkcod = dr["STKCOD"].ToString();
                    string price = dr["Price"].ToString();

                    decimal priceValue;
                    if (!decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out priceValue)) {
                        priceValue = 0m;
                    }

                    string signature = PriceSignatureHelper.GenerateSignature(stkcod, priceValue, signatureTimestamp);

                    Getdata.Add(new {
                        ItemNo = dr["STKCOD"].ToString(),
                        Description = dr["Stkdes"].ToString(),
                        Brand = dr["Brand"].ToString(),
                        Company = dr["Company"].ToString(),
                        Price = dr["Price"].ToString(),
                        SalePrice = dr["PrcPrice"].ToString(),
                        PlcPrice = dr["PlcPrice"].ToString(),
                        SpcPrice = dr["SpcPrice"].ToString(),
                        PromotionCode = dr["PromotionCode"].ToString(),
                        PromoDesc = dr["PromoDesc"].ToString(),
                        PromoPrice = dr["PromoPrice"].ToString(),
                        Available_Stock = dr["Available_Stock"].ToString(),
                        NewItem = dr["New Item"].ToString(),
                        FavoriteItem = dr["Favorite Item"].ToString(),
                        PATH = Path.Combine(root, dr["IMAGE_NAME"].ToString()),
                        Inactive = dr["Inactive"].ToString(),
                        minord = dr["minord"].ToString(),
                        maxord = dr["maxord"].ToString(),
                        spackuom = dr["SPackUOM"].ToString(),
                        prclst_no = dr["prclst_no"].ToString(),
                        Stock = dr["Stock"].ToString(),
                        ItemClass = dr["ItemClass"].ToString(),
                        Timestamp = signatureTimestamp,
                        Signature = signature
                    });
                }
            }
            catch (Exception ex) {
                message = ex.Message;
                // TODO: ควร log message ไว้ตรวจสอบ (Logger.Error(message))
            }
            finally {
                // ปิด/dispose ทุกอย่างไม่ว่าจะสำเร็จหรือ exception ก็ตาม
                if (dr != null && !dr.IsClosed) {
                    dr.Close();
                    dr.Dispose();
                }
                if (command != null) {
                    command.Dispose();
                }
                if (Connection.State != ConnectionState.Closed) {
                    Connection.Close();
                }
                Connection.Dispose();
            }

            return Json(Getdata, JsonRequestBehavior.AllowGet);
        }
    }
}
