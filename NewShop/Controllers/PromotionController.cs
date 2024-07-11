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
            ViewBag.slmCodeList = GetSalesmanName(user);
            ViewBag.pmCodeList = GetProductName(user);
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
        public List<SelectListItem> GetProductName(string pmCode)
        {
            List<SelectListItem> productList = new List<SelectListItem>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var  command = new SqlCommand("P_Price_Approve_Data", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUsrID", pmCode);
            command.Parameters.AddWithValue("@inType", 4);
            //command.ExecuteNonQuery();
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
    }
}
