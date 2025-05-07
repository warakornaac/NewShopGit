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

    }
}
