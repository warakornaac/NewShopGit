using NewShop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        public async Task<JsonResult> SendOtp(string phone, string user, string refer)
        {
            string otp = new string(Enumerable.Repeat(_otpChars, 6)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
            string reff = GenerateRandomString(4);
            string status = string.Empty;
            string message = string.Empty;
            //string statusApi = string.Empty;
            string Api = string.Empty;
            var connectString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectString);
            try
            {
                conn.Open();
                var command = new SqlCommand("P_ADD_OTP", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@user", user.Trim());
                command.Parameters.AddWithValue("@Phone", phone.Trim());
                command.Parameters.AddWithValue("@ref", refer.Trim());
                command.Parameters.AddWithValue("@OTP", otp.Trim());
                SqlParameter p = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                p.Direction = ParameterDirection.Output;
                SqlParameter m = new SqlParameter("@outColumn", SqlDbType.NVarChar, 100);
                m.Direction = ParameterDirection.Output;
                command.Parameters.Add(p);
                command.Parameters.Add(m);
                command.ExecuteNonQuery();
                message = command.Parameters["@outColumn"].Value.ToString();
                status = command.Parameters["@outGenstatus"].Value.ToString();
                command.Dispose();
                //var statusApi = Apiservice(phone, user, otp, reff);
                //Api = await statusApi;
                Api = "YES";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            conn.Close();

            return Json(new { message = message, status = status, Apisend = Api }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Verify(string lineid, string phone, string user, string otp, string refer, string page)
        {
            var message = string.Empty;
            var connectString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectString);
            try
            {
                conn.Open();
                var command = new SqlCommand("P_CHECK_OTP", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@lineid", lineid.Trim());
                command.Parameters.AddWithValue("@user", user.Trim());
                command.Parameters.AddWithValue("@Phone", phone.Trim());
                command.Parameters.AddWithValue("@OTP", otp.Trim());
                command.Parameters.AddWithValue("@ref", refer.Trim());
                SqlParameter p = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                p.Direction = ParameterDirection.Output;
                command.Parameters.Add(p);
                command.ExecuteNonQuery();
                message = command.Parameters["@outGenstatus"].Value.ToString();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, page = page }, JsonRequestBehavior.AllowGet);
        }
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return stringBuilder.ToString();
        }
        private async Task<string> Apiservice(string phone, string user, string otp, string reff)
        {
            var urlAPI = "https://localhost:44361/Post/Sms";
            var post = new SmsModels
            {
                Phone = phone,
                Otp = otp,
                Ref = reff,
                User = user
            };
            try
            {
                // ตั้งค่าการตรวจสอบใบรับรอง SSL/TLS
                var handler = new HttpClientHandler();
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlAPI, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




    }
}
