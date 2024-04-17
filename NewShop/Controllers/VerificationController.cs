using NewShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewShop.Controllers
{
    public class VerificationController : Controller
    {
        //
        // GET: /Verification/

        private readonly string _otpChars = "0123456789";
        private readonly Random _random = new Random();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendOtp(string phone, string cuscode)
        {
            string otp = new string(Enumerable.Repeat(_otpChars, 6)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
            string message = string.Empty;
            var connectString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectString);
            try
            {
                conn.Open();
                var command = new SqlCommand("P_ADD_OTP", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Cuscod", cuscode);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@OTP", otp);
                command.ExecuteNonQuery();
                command.Dispose();

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            conn.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Verify(string phone, string cuscode)
        {

            return Json("");
        }

    }
}
