using NewShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace NewShop.Controllers
{
    public class ManagementCustomerportalUserController : Controller
    {
        //
        // GET: /ManageCustomerportalUser/

        public ActionResult Index()
        {
            string message = string.Empty;
            List<customerPortalUser> getData = new List<customerPortalUser>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_GetUser_CustomerPortal", Connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    getData.Add(new customerPortalUser()
                    {
                        slmcod = Reader["slmcod"] != DBNull.Value ? Reader["slmcod"].ToString() : "",
                        Email = Reader["Email"] != DBNull.Value ? Reader["Email"].ToString() : "",
                        Cuscode = Reader["CusCode"] != DBNull.Value ? Reader["CusCode"].ToString() : ""
                    });
                }
                ViewBag.Getdata = getData;
                Reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return View();
        }
        public ActionResult UserData(string CUSCOD, string Email, string NewCUSCOD)
        {
            string message = string.Empty;
            List<customerPortalUser> getData = new List<customerPortalUser>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                var command = new SqlCommand("P_GetUser_CustomerPortal", Connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    getData.Add(new customerPortalUser()
                    {
                        slmcod = Reader["slmcod"] != DBNull.Value ? Reader["slmcod"].ToString() : "",
                        Email = Reader["Email"] != DBNull.Value ? Reader["Email"].ToString() : "",
                        Cuscode = Reader["CusCode"] != DBNull.Value ? Reader["CusCode"].ToString() : ""
                    });
                }
                ViewBag.Getdata = getData;
                Reader.Close();
                command.Dispose();
                Connection.Close();
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return PartialView("Index", new
            {
                ViewBag.Getdata
            });
        }



    }
}
