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
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NewShop.Controllers
{
    public class SendDeliveryStatusController : Controller
    {
        // GET: /SendDeliveryStatus/
        public ActionResult Index()
        {
            //var getDateInput = Utils.PushMessage();
            List<ListSendDelivery> List = new List<ListSendDelivery>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("p_Order_Notify", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new NewShop.Models.ListSendDelivery()
                {
                    Uid = dr["Uid"].ToString(),
                    Docno = dr["Ord_DocNo"].ToString(),
                    Docdate = dr["ORD_Date"].ToString(),
                    Cusname = dr["CUSNAM"].ToString(),
                    Delivery = dr["Notify"].ToString(),
                    User = "superadmin",
                });
            }

            ViewBag.listData = List;
            return View();
        }
        public ActionResult DashboardSendDeliveryStatus()
        {
            int numSuccess = 0;
            int numError = 0;
            string[] arrSuccess = new string[2];
            string[] arrError = new string[2];


            ViewBag.listData = ""; // GetStoreSearchOrderNotify();
            ViewBag.countlistDataAll = "";// GetStoreSearchOrderNotify();
            //get data by stored
            //if (List.Any())
            //{
            //    foreach (var rowList in List)
            //    {
            //        var statusApi = ApiPushMessage(rowList.Uid, rowList.Docno, rowList.Docdate, rowList.Cusname, rowList.Delivery, "Warakorn.pra");
            //        string json = JsonConvert.SerializeObject(statusApi.Result.Data);
            //        ResultApi dto = JsonConvert.DeserializeObject<ResultApi>(json);
            //        //send fail
            //        if (dto.status != "OK")
            //        {
            //            ++numError;
            //            //arrError[numError] = rowList.Docno;
            //        }
            //        else
            //        {
            //            ++numSuccess;
            //            //arrSuccess[numSuccess] = rowList.Docno;
            //        }
            //    }
            //}
            ViewBag.numError = numError;
            ViewBag.arrError = arrError;
            ViewBag.numSuccess = numSuccess;
            ViewBag.arrSuccess = arrSuccess;

            return View();
        }
        [HttpPost]
        public JsonResult GetSendDeliveryCount()
        {
            var getNotifyCount = GetStoreSearchOrderNotifyCount();
            return Json(getNotifyCount, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SearchSendDeliveryStatusAll()
        {
            int numSuccess = 0;
            int numError = 0;
            string[] arrSuccess = new string[2];
            string[] arrError = new string[2];

            List<ListSendDelivery> List = new List<ListSendDelivery>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("p_Order_Notify", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new NewShop.Models.ListSendDelivery()
                {
                    Uid = "Ucea94914394b7928e1c8dd37541d682a".ToString(),
                    Docno = dr["Ord_DocNo"].ToString(),
                    Docdate = dr["ORD_Date"].ToString(),
                    Cusname = dr["CUSNAM"].ToString(),
                    Delivery = dr["Notify"].ToString(),
                    User = "superadmin",
                });
            }

            ViewBag.listData = List;
            ViewBag.countlistDataAll = List.Count;
            //get data by stored
            if (List.Any())
            {
                foreach (var rowList in List)
                {
                    var statusApi = ApiPushMessage(rowList.Uid, rowList.Docno, rowList.Docdate, rowList.Cusname, rowList.Delivery, "Warakorn.pra");
                    string json = JsonConvert.SerializeObject(statusApi.Result.Data);
                    ResultApi dto = JsonConvert.DeserializeObject<ResultApi>(json);
                    //send fail
                    if (dto.status != "OK")
                    {
                        ++numError;
                        //arrError[numError] = rowList.Docno;
                    }
                    else
                    {
                        ++numSuccess;
                        //arrSuccess[numSuccess] = rowList.Docno;
                    }
                }
            }
            ViewBag.numError = numError;
            ViewBag.arrError = arrError;
            ViewBag.numSuccess = numSuccess;
            ViewBag.arrSuccess = arrSuccess;

            return PartialView("_listDataSendDeliveryStatusAll", new
            {
                @ViewBag.listData,
                @ViewBag.countlistDataAll,
                @ViewBag.numError,
                @ViewBag.arrError,
                @ViewBag.numSuccess,
                @ViewBag.arrSuccess
            });
        }
        //ส่ง
        public ActionResult SendDeliveryStatusAuto()
        {
            int numSuccess = 0;
            int numError = 0;
            string[] arrSuccess = new string[2];
            string[] arrError = new string[2];

            List<ListSendDelivery> List = new List<ListSendDelivery>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("p_Order_Notify", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new NewShop.Models.ListSendDelivery()
                {
                    Uid = "Ucea94914394b7928e1c8dd37541d682a".ToString(),
                    Docno = dr["Ord_DocNo"].ToString(),
                    Docdate = dr["ORD_Date"].ToString(),
                    Cusname = dr["CUSNAM"].ToString(),
                    Delivery = dr["Notify"].ToString(),
                    User = "superadmin",
                });
            }

            ViewBag.listData = List;
            ViewBag.countlistDataAll = List.Count;
            //get data by stored
            if (List.Any())
            {
                foreach (var rowList in List)
                {
                    var statusApi = ApiPushMessage(rowList.Uid, rowList.Docno, rowList.Docdate, rowList.Cusname, rowList.Delivery, "Warakorn.pra");
                    string json = JsonConvert.SerializeObject(statusApi.Result.Data);
                    ResultApi dto = JsonConvert.DeserializeObject<ResultApi>(json);
                    //send fail
                    if (dto.status != "OK")
                    {
                        ++numError;
                        //arrError[numError] = rowList.Docno;
                    }
                    else
                    {
                        ++numSuccess;
                        //arrSuccess[numSuccess] = rowList.Docno;
                    }
                }
            }
            ViewBag.numError = numError;
            ViewBag.arrError = arrError;
            ViewBag.numSuccess = numSuccess;
            ViewBag.arrSuccess = arrSuccess;

            return PartialView("_listDataSendDeliveryStatusAll", new
            {
                @ViewBag.listData,
                @ViewBag.countlistDataAll,
                @ViewBag.numError,
                @ViewBag.arrError,
                @ViewBag.numSuccess,
                @ViewBag.arrSuccess
            });
        }
        //list order
        public JsonResult GetStoreSearchOrderNotify()
        {
            List<ListSendDelivery> ListSendDelivery = new List<ListSendDelivery>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("p_Order_Notify", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                ListSendDelivery.Add(new NewShop.Models.ListSendDelivery()
                {
                    Uid = dr["Uid"].ToString(),
                    Docno = dr["Ord_DocNo"].ToString(),
                    Docdate = dr["ORD_Date"].ToString(),
                    Cusname = dr["CUSNAM"].ToString(),
                    Delivery = dr["Notify"].ToString(),
                    User = "System",
                });
            }
            return Json(ListSendDelivery, JsonRequestBehavior.AllowGet);
        }
        //notify count
        [HttpPost]
        public JsonResult GetStoreSearchOrderNotifyCount()
        {
            List<ListSendDeliveryCount> ListSendDeliveryCount = new List<ListSendDeliveryCount>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("p_Order_Notify_Count", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                ListSendDeliveryCount.Add(new ListSendDeliveryCount()
                {
                    sumOrderAll = dr["sumOrderAll"].ToString(),
                    sumOrderCurrentDate = dr["sumOrderCurrentDate"].ToString(),
                    sumOrderByMonth = dr["sumOrderByMonth"].ToString(),
                    sumOrderStatus1 = dr["sumOrderStatus1"].ToString(),
                    sumOrderStatus2 = dr["sumOrderStatus2"].ToString(),
                    sumOrderStatus3 = dr["sumOrderStatus3"].ToString(),
                    sumOrderStatus4 = dr["sumOrderStatus4"].ToString(),
                    sumDateCurrent = DateTime.Now.ToString(),
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //return Json(List, JsonRequestBehavior.AllowGet);
            //return Json(new { data = ListSendDeliveryCount });
            return Json(ListSendDeliveryCount, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> ApiPushMessage(string Uid, string Docno, string Docdate, string Cusname, string Delivery, string User)
        {
            string status = string.Empty;
            string message = string.Empty;
            var url = "https://mst.aac.co.th/APIService/Post/PushMessage";
            var post = new ListSendDelivery
            {
                Uid = Uid,
                Docno = Docno,
                Docdate = Docdate,
                Cusname = Cusname,
                Delivery = Delivery,
                User = User
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    message = responseContent;
                    status = response.StatusCode.ToString();
                }
                else
                {
                    status =  response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { status = status, message = message}, JsonRequestBehavior.AllowGet);
        }
        public class ResultApi
        {
            public string status { get; set; }
            public string message { get; set; }
        }

    }
}
