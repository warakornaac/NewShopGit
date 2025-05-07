using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewShop.Controllers
{
    public class OrderTrackingController : Controller
    {
        //
        // GET: /OrderTracking/

        public ActionResult Index()
        {
            var Getdata = new List<object>();
            string message = string.Empty;
            if (true)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                try
                {
                    var cmd = new SqlCommand("Select [Lookup ID],[Description] from v_CDEL", conn);
                    int result = cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Getdata.Add(new
                        {
                            ID = reader["Lookup ID"] != DBNull.Value ? reader["Lookup ID"].ToString() : string.Empty,
                            Value = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                        });
                    }
                    reader.Close();
                    reader.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
                ViewBag.OrderTracking = Getdata;
                return Json(Getdata, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public JsonResult GetReason(string flag)
        {
            if (string.IsNullOrWhiteSpace(flag))
            {
                return Json(new
                {
                    StatusId = "400",
                    Error = "Bad Request",
                    Message = "flag parameter is required."
                }, JsonRequestBehavior.AllowGet);
            }
            var Getdata = new List<object>();
            string message = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("P_Get_OrderTracking", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inflag", flag);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Getdata.Add(new
                                {
                                    Id = reader["Id"]?.ToString(),
                                    Value = reader["Value"]?.ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        StatusId = "500",
                        Error = "Internal Server Error",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(Getdata, JsonRequestBehavior.AllowGet);
        }


    }
}
