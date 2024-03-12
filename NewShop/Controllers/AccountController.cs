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
using System.Web.Services.Protocols;
using System.Security.Principal;
using System.DirectoryServices;
using System.Web.Security;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace NewShop.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            if (this.Session["UserType"] == null)
            {
                this.Session["UserType"] = "";
            }


            return View();

        }
        [HttpGet]
        public ActionResult LogIn()
        {
            if (this.Session["UserType"] == null)
            {
                this.Session["UserType"] = "";
            }
            return View();
            //}
        }
        [HttpGet]
        public ActionResult CheckLoginExternal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckDataLoginExternal(string userId, string email, string displayName)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Check_Login_External", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@displayName", displayName);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);

                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataLoginExternal(string userId, string page)
        {
            this.Session["UserID"] = string.Empty;
            this.Session["UserType"] = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select * From UsrTbl_line where UserIdLine =N'", Connection);
                SqlDataReader rev = cmd.ExecuteReader();
                while (rev.Read())
                {
                    if (!string.IsNullOrEmpty(rev["UsrID"].ToString()))
                    {
                        message = "Y";
                        this.Session["UserID"] = rev["UsrID"].ToString();
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        //get sesssion
                        string sessionId = string.Empty;
                        string httpCookie = string.Empty;
                        if (Request.ServerVariables["HTTP_COOKIE"] != null)
                        {
                            httpCookie = Request.ServerVariables["HTTP_COOKIE"].Substring(0, (Request.ServerVariables["HTTP_COOKIE"].Length > 399) ? 399 : Request.ServerVariables["HTTP_COOKIE"].Length);
                        }
                        sessionId = httpCookie;

                        if (sessionId != null)
                        {
                            sessionId = sessionId.Substring(sessionId.Length - 24);
                            this.Session["ID"] = sessionId;
                        }
                        else
                        {
                            this.Session["ID"] = "775.333";
                        }
                        //set session id
                        var command = new SqlCommand("P_logSingin_customer_mobileStatus", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UsrID", userId);
                        command.Parameters.AddWithValue("@SessionId", sessionId);
                        command.Parameters.AddWithValue("@flag", "external");
                        command.ExecuteReader();
                        command.Dispose();
                    }
                }
                rev.Close();
                rev.Dispose();
                cmd.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var returnField = new { UserId = this.Session["UserID"], UserType = this.Session["UserType"], ID = this.Session["ID"], page = page, message = message };
            return Json(returnField, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LogIn(LoginUserViewModel User)
        {
            string Userlog = string.Empty;
            string Usertype = string.Empty;
            string dateexpire = string.Empty;
            string UsrClmStaff = string.Empty;
            int intdateexpire = 0;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            //Connection.Open();
            // GenericIdentity identity = null;
            Connection.Open();
            try
            {
                this.Session["UserType"] = null;
                this.Session["UserID"] = User.Usre;
                this.Session["UserPassword"] = User.Password;
                this.Session["UsrGrpspecial"] = 0;
                this.Session["DatetoExpire"] = "..";
                this.Session["UsrClmStaff"] = "0";
                string UserType = string.Empty;
                string sessionId = Request["http_cookie"];
                string secCodeArr = string.Empty;
                SqlCommand cmdcus = new SqlCommand("select * From v_UsrTbl_catalog where UsrID =N'" + User.Usre + "'and [dbo].F_decrypt([Password])='" + User.Password + "' and  [LoginFail] <> 3", Connection);
                SqlDataReader revcus = cmdcus.ExecuteReader();
                while (revcus.Read())
                {

                    this.Session["UserType"] = revcus["UsrTyp"].ToString();
                    this.Session["Department"] = revcus["Department"].ToString();
                    this.Session["CUSCOD"] = revcus["CUSCOD"].ToString();
                    UserType = Session["UserType"].ToString();
                    this.Session["UsrClmStaff"] = revcus["UsrClmStaff"].ToString();

                    sessionId = sessionId.Substring(sessionId.Length - 24);
                    this.Session["ID"] = sessionId;
                }

                revcus.Close();
                revcus.Dispose();
                cmdcus.Dispose();
                FormsAuthentication.SetAuthCookie(User.Usre, false);


                if (UserType == null)
                {
                    //ADSRV01
                    DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User.Usre, User.Password);
                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(SAMAccountName=" + User.Usre + ")";
                    search.PropertiesToLoad.Add("cn");

                    SearchResult result = search.FindOne();
                    //result.GetDirectoryEntry();
                    // Connection.Open();
                    if (null == result)
                    {
                        if (IsValid(User.Usre, User.Password))
                        {

                        }
                        else
                        {
                            ModelState.AddModelError("", "Login details are wrong.");
                        }
                        //throw new SoapException("Error authenticating user.",SoapException.ClientFaultCode);
                    }
                    else
                    {
                        this.Session["UserID"] = User.Usre;
                        this.Session["UserPassword"] = User.Password;
                        this.Session["UsrGrpspecial"] = 0;
                        SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
                        SqlDataReader rev = cmd.ExecuteReader();
                        while (rev.Read())
                        {

                            dateexpire = rev["Date to Expire"].ToString();
                            //dateexpire = "2";
                            this.Session["UserType"] = rev["UsrTyp"].ToString();
                            this.Session["CUSCOD"] = "";
                            this.Session["Department"] = rev["Department"].ToString();
                        }
                        rev.Close();
                        rev.Dispose();
                        cmd.Dispose();

                        intdateexpire = Convert.ToInt32(dateexpire);
                        this.Session["expdatecal"] = intdateexpire;
                        if (intdateexpire <= 15)
                        {
                            this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
                        }
                        else if (intdateexpire == 0)
                        {
                            this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
                        }
                        else
                        {

                            this.Session["DatetoExpire"] = "..";
                        }

                        FormsAuthentication.SetAuthCookie(User.Usre, false);

                        //return RedirectToAction("Index", "SeleScrCustomer");
                        UserType = Session["UserType"].ToString();
                        if (UserType == "5")
                        {
                            // return RedirectToAction("Index", "Home");
                            return RedirectToAction("Index", "PriceApproval");
                        }
                        else
                        {
                            return RedirectToAction("dashboard", "SeleScrCustomer");
                        }
                    }

                    // ModelState.AddModelError("", "Login details are wrong.");
                }
                else if (UserType == "")
                {
                    //ADSRV01
                    DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User.Usre, User.Password);
                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = "(SAMAccountName=" + User.Usre + ")";
                    search.PropertiesToLoad.Add("cn");

                    SearchResult result = search.FindOne();
                    //result.GetDirectoryEntry();
                    // Connection.Open();
                    if (null == result)
                    {
                        if (IsValid(User.Usre, User.Password))
                        {

                        }
                        else
                        {
                            ModelState.AddModelError("", "Login details are wrong.");
                        }
                        //throw new SoapException("Error authenticating user.",SoapException.ClientFaultCode);
                    }
                    else
                    {
                        this.Session["UserID"] = User.Usre;
                        this.Session["UserPassword"] = User.Password;
                        this.Session["UsrGrpspecial"] = 0;
                        SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
                        SqlDataReader rev = cmd.ExecuteReader();
                        while (rev.Read())
                        {

                            dateexpire = rev["Date to Expire"].ToString();
                            //dateexpire = "2";
                            this.Session["UserType"] = rev["UsrTyp"].ToString();
                            this.Session["CUSCOD"] = "";
                            this.Session["Department"] = rev["Department"].ToString();
                        }
                        rev.Close();
                        rev.Dispose();
                        cmd.Dispose();

                        intdateexpire = Convert.ToInt32(dateexpire);
                        this.Session["expdatecal"] = intdateexpire;
                        if (intdateexpire <= 15)
                        {
                            this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
                        }
                        else if (intdateexpire == 0)
                        {
                            this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
                        }
                        else
                        {

                            this.Session["DatetoExpire"] = "..";
                        }

                        FormsAuthentication.SetAuthCookie(User.Usre, false);

                        //return RedirectToAction("Index", "SeleScrCustomer");
                        UserType = Session["UserType"].ToString();
                        if (UserType == "5")
                        {
                            // return RedirectToAction("Index", "Home");
                            return RedirectToAction("Index", "PriceApproval");
                        }
                        else
                        {
                            return RedirectToAction("dashboard", "SeleScrCustomer");
                        }
                    }
                    // ModelState.AddModelError("", "Login details are wrong.");
                }
                else if (UserType == "6") //customer
                {
                    var command = new SqlCommand("P_logSingin", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@pCusCod", strcustome);
                    command.Parameters.AddWithValue("@UsrID", User.Usre);
                    command.Parameters.AddWithValue("@SessionId", sessionId);
                    command.ExecuteReader();
                    command.Dispose();
                    //get date expire
                    SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
                    SqlDataReader rev = cmd.ExecuteReader();
                    while (rev.Read())
                    {
                        dateexpire = rev["Date to Expire"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd.Dispose();
                    intdateexpire = Convert.ToInt32(dateexpire);
                    this.Session["expdatecal"] = intdateexpire;
                    if (intdateexpire <= 0)
                    {
                        ModelState.AddModelError("", "Your password expire.");
                    }
                    else
                    {
                        return RedirectToAction("dashboard", "SeleScrCustomer");
                    }
                    //end get date expire
                    //return RedirectToAction("Index", "Home");
                }
                else if (UserType == "2")//sales
                {
                    return RedirectToAction("dashboard", "SeleScrCustomer");
                }
                else if (UserType == "1")//salesco
                {
                    return RedirectToAction("dashboard", "SeleScrCustomer");
                }
                else if (UserType == "5")//pm
                {
                    return RedirectToAction("Index", "PriceApproval");
                }



            }
            catch (COMException ex)
            {

                ModelState.AddModelError("", "Login details are wrong.");

            }
            Connection.Close();

            return View();
            // return View(User);
            //  return User.Usre;
        }
        //public ActionResult LogIn(LoginUserViewModel User)
        //{
        //    string Userlog = string.Empty;
        //    string Usertype = string.Empty;
        //    string dateexpire = string.Empty;
        //    int intdateexpire = 0;
        //    var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
        //    SqlConnection Connection = new SqlConnection(connectionString);
        //    //Connection.Open();
        //    // GenericIdentity identity = null;
        //    Connection.Open();
        //    try
        //    {
        //        //ADSRV01
        //        DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User.Usre, User.Password);
        //        DirectorySearcher search = new DirectorySearcher(entry);
        //        search.Filter = "(SAMAccountName=" + User.Usre + ")";
        //        search.PropertiesToLoad.Add("cn");

        //        SearchResult result = search.FindOne();
        //        //result.GetDirectoryEntry();
        //       // Connection.Open();
        //        if (null == result)
        //        {
        //            if (IsValid(User.Usre, User.Password))
        //            {

        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Login details are wrong.");
        //            }
        //            //throw new SoapException("Error authenticating user.",SoapException.ClientFaultCode);
        //        }
        //        else
        //        {
        //            this.Session["UserID"] = User.Usre;
        //            this.Session["UserPassword"] = User.Password;
        //            this.Session["UsrGrpspecial"] = 0;
        //            SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
        //            SqlDataReader rev = cmd.ExecuteReader();
        //            while (rev.Read())
        //            {

        //                dateexpire = rev["Date to Expire"].ToString();
        //                //dateexpire = "2";
        //                this.Session["UserType"] = rev["UsrTyp"].ToString();
        //                this.Session["CUSCOD"] = "";
        //                this.Session["Department"] = rev["Department"].ToString();
        //            }
        //            rev.Close();
        //            rev.Dispose();
        //            cmd.Dispose();

        //            intdateexpire = Convert.ToInt32(dateexpire);
        //            this.Session["expdatecal"] = intdateexpire;
        //            if (intdateexpire <= 15)
        //            {
        //                this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
        //            }
        //            else if (intdateexpire == 0)
        //            {
        //                this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
        //            }
        //            else
        //            {

        //                this.Session["DatetoExpire"] = "..";
        //            }

        //            FormsAuthentication.SetAuthCookie(User.Usre, false);

        //            //return RedirectToAction("Index", "SeleScrCustomer");
        //            string UserType = Session["UserType"].ToString();
        //            if (UserType == "5")
        //            {
        //               // return RedirectToAction("Index", "Home");
        //                return RedirectToAction("Index", "PriceApproval");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "SeleScrCustomer");
        //            }
        //        }

        //    }
        //    catch (COMException ex)
        //    {
        //        this.Session["UserType"] = null;
        //        this.Session["UserID"] = User.Usre;
        //        this.Session["UserPassword"] = User.Password;
        //        this.Session["UsrGrpspecial"] = 0;
        //        this.Session["DatetoExpire"] = "..";
        //        string UserType = string.Empty;
        //        SqlCommand cmdcus = new SqlCommand("select * From v_UsrTbl_catalog where UsrID =N'" + User.Usre + "'and [dbo].F_decrypt([Password])='" + User.Password + "' and  [LoginFail] <> 3", Connection);
        //        SqlDataReader revcus = cmdcus.ExecuteReader();
        //        while (revcus.Read())
        //        {

        //            this.Session["UserType"] = revcus["UsrTyp"].ToString();
        //            this.Session["Department"] = revcus["Department"].ToString();
        //            this.Session["CUSCOD"] = revcus["CUSCOD"].ToString();
        //            UserType = Session["UserType"].ToString();
        //        }

        //        revcus.Close();
        //        revcus.Dispose();
        //        cmdcus.Dispose();
        //        FormsAuthentication.SetAuthCookie(User.Usre, false);

        //        if (UserType == null)
        //        {
        //            ModelState.AddModelError("", "Login details are wrong.");
        //        }else if (UserType == "")
        //        {
        //            ModelState.AddModelError("", "Login details are wrong.");
        //        }
        //        else if (UserType == "6") //customer
        //        {
        //            //return RedirectToAction("Index", "Home");
        //            return RedirectToAction("Index", "SeleScrCustomer");
        //        }
        //        else if (UserType == "2")//sales
        //        {
        //            return RedirectToAction("Index", "SeleScrCustomer");
        //        }
        //        else if (UserType == "1")//salesco
        //        {
        //            return RedirectToAction("Index", "SeleScrCustomer");
        //        }
        //        else if (UserType == "5")//pm
        //        {
        //            return RedirectToAction("Index", "PriceApproval");
        //        }


        //    }
        //    Connection.Close();

        //   return View();
        //    // return View(User);
        //    //  return User.Usre;
        //}

        //private bool IsValid(string p1,string p2)
        //{
        //    throw new NotImplementedException();
        //}

        // [HttpPost]
        public ActionResult LogInRedir(string User, string password)
        {
            string Docdisplay = string.Empty;
            string Userlog = string.Empty;
            string Usertype = string.Empty;
            string dateexpire = string.Empty;
            // string User = string.Empty;
            // string password = string.Empty;
            int intdateexpire = 0;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            //Connection.Open();
            // GenericIdentity identity = null;
            Connection.Open();
            try
            {


                //string basePack = Pack;

                //byte[] data = System.Convert.FromBase64String(Docdisplay);

                //ADSRV01
                DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User, password);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + User + ")";
                search.PropertiesToLoad.Add("cn");

                SearchResult result = search.FindOne();
                //result.GetDirectoryEntry();
                // Connection.Open();
                if (null == result)
                {
                    if (IsValid(User, password))
                    {

                    }
                    else
                    {
                        ModelState.AddModelError("", "Login details are wrong.");
                    }
                    //throw new SoapException("Error authenticating user.",SoapException.ClientFaultCode);
                }
                else
                {
                    this.Session["UserID"] = User;
                    this.Session["UserPassword"] = password;
                    this.Session["UsrGrpspecial"] = 0;
                    SqlCommand cmd = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User + "'", Connection);
                    SqlDataReader rev = cmd.ExecuteReader();
                    while (rev.Read())
                    {

                        dateexpire = rev["Date to Expire"].ToString();
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        this.Session["CUSCOD"] = "";
                        this.Session["Department"] = rev["Department"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd.Dispose();

                    intdateexpire = Convert.ToInt32(dateexpire);
                    this.Session["expdatecal"] = intdateexpire;
                    if (intdateexpire <= 15)
                    {
                        this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
                    }
                    else if (intdateexpire == 0)
                    {
                        this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
                    }
                    else
                    {

                        this.Session["DatetoExpire"] = "..";
                    }

                    FormsAuthentication.SetAuthCookie(User, false);

                    //return RedirectToAction("Index", "SeleScrCustomer");
                    string UserType = Session["UserType"].ToString();
                    if (UserType == "5")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SeleScrCustomer");
                    }
                }

            }
            catch (COMException ex)
            {
                this.Session["UserType"] = null;
                this.Session["UserID"] = User;
                this.Session["UserPassword"] = password;
                this.Session["UsrGrpspecial"] = 0;
                string UserType = string.Empty;
                SqlCommand cmdcus = new SqlCommand("select * From v_UsrTbl_catalog where UsrID =N'" + User + "'and [dbo].F_decrypt([Password])='" + password + "' and  [LoginFail] <> 3", Connection);
                SqlDataReader revcus = cmdcus.ExecuteReader();
                while (revcus.Read())
                {

                    this.Session["UserType"] = revcus["UsrTyp"].ToString();
                    this.Session["Department"] = revcus["Department"].ToString();
                    this.Session["CUSCOD"] = revcus["CUSCOD"].ToString();
                    UserType = Session["UserType"].ToString();
                }

                revcus.Close();
                revcus.Dispose();
                cmdcus.Dispose();
                FormsAuthentication.SetAuthCookie(User, false);

                if (UserType == null)
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
                else if (UserType == "")
                {
                    ModelState.AddModelError("", "Login details are wrong.");
                }
                else if (UserType == "6") //customer
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (UserType == "2")//sales
                {
                    return RedirectToAction("Index", "SeleScrCustomer");
                }
                else if (UserType == "1")//salesco
                {
                    return RedirectToAction("Index", "SeleScrCustomer");
                }
                else if (UserType == "5")//pm
                {
                    return RedirectToAction("Index", "Home");
                }


            }
            Connection.Close();

            return View();
            // return View(User);
            //  return User.Usre;
        }
        /*
        //External LogIn
        [HttpPost]
        public ActionResult CheckDataLoginExternal(string userId, string email, string displayName)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("สโตเช็ค Login", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@displayName", displayName);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);

                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataLoginExternal(string userId, string page)
        {
            this.Session["UserID"] = string.Empty;
            this.Session["UserType"] = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                //SqlCommand cmd = new SqlCommand("select * From UsrTbl_Line where UserIdLine =N'" + userId + "' and  [LoginFail] <> 3", Connection);
                SqlCommand cmd = new SqlCommand("select * From UsrTbl_Line where UserIdLine =N'" + userId, Connection);
                SqlDataReader rev = cmd.ExecuteReader();
                while (rev.Read())
                {
                    if (!string.IsNullOrEmpty(rev["UsrID"].ToString()))
                    {
                        message = "Y";
                        this.Session["UserID"] = rev["UsrID"].ToString();
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        //get sesssion
                        string sessionId = string.Empty;
                        string httpCookie = string.Empty;
                        if (Request.ServerVariables["HTTP_COOKIE"] != null)
                        {
                            httpCookie = Request.ServerVariables["HTTP_COOKIE"].Substring(0, (Request.ServerVariables["HTTP_COOKIE"].Length > 399) ? 399 : Request.ServerVariables["HTTP_COOKIE"].Length);
                        }
                        sessionId = httpCookie;

                        if (sessionId != null)
                        {
                            sessionId = sessionId.Substring(sessionId.Length - 24);
                            this.Session["ID"] = sessionId;
                        }
                        else
                        {
                            this.Session["ID"] = "775.333";
                        }
                        //set session id
                        var command = new SqlCommand("สโตคัสตอมเมอร์ Login", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UsrID", userId);
                        command.Parameters.AddWithValue("@SessionId", sessionId);
                        command.Parameters.AddWithValue("@flag", "external");
                        command.ExecuteReader();
                        command.Dispose();
                    }
                }
                rev.Close();
                rev.Dispose();
                cmd.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var returnField = new { UserId = this.Session["UserID"], UserType = this.Session["UserType"], ID = this.Session["ID"], page = page, message = message };
            return Json(returnField, JsonRequestBehavior.AllowGet);
        }


        */


        [HttpPost]
        public ActionResult ChangePassword(string userName, string oldPassword, string newPassword)
        {
            //string userName = System.Environment.UserName;
            //string userName = "Deploy";
            //currentPassword = "Happy1002";
            //newPassword = "Happy1003";
            string messageSave = "You password has been changed.";
            string statusSave = "success";
            try
            {
                DirectoryEntry directionEntry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", userName, oldPassword);
                if (directionEntry != null)
                {
                    DirectorySearcher search = new DirectorySearcher(directionEntry);
                    search.Filter = "(SAMAccountName=" + userName + ")";
                    SearchResult result = search.FindOne();
                    if (result != null)
                    {
                        DirectoryEntry userEntry = result.GetDirectoryEntry();
                        if (userEntry != null)
                        {
                            userEntry.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
                            // userEntry.Invoke("SetPassword", new object[] { newPassword }); กรณี reset pass สิทธิ์ admin
                            userEntry.CommitChanges();
                            userEntry.Close();
                            userEntry.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Exception has been thrown by the target of an invocation / passwprd new ไม่ผ่าน
                statusSave = "error";
                messageSave = ex.Message.ToString();//ex.Message.ToString();
            }
            return Json(new { status = statusSave, message = messageSave }, JsonRequestBehavior.AllowGet);
        }
        private bool IsValid(string user, string Password)
        {

            bool IsValid = false;
            if (user == null || Password == null) { IsValid = false; }
            else
            {


            }
            return IsValid;
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(string email, string tel, string line, string cuscos, string user)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            string lindId = " ";
            string displayName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("P_Register_customer", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // cmd.Parameters.AddWithValue("@inpassword", password.Trim());
                    cmd.Parameters.AddWithValue("@inemail", email.Trim());
                    cmd.Parameters.AddWithValue("@intel", tel.Trim()); ;
                    cmd.Parameters.AddWithValue("@inlineid", line.Trim());
                    cmd.Parameters.AddWithValue("@inuserId", lindId.Trim());
                    cmd.Parameters.AddWithValue("@indisplayName", displayName.Trim());
                    cmd.Parameters.AddWithValue("@incuscod", cuscos);
                    cmd.Parameters.AddWithValue("@inUser", user.Trim());
                    int INSID = cmd.ExecuteNonQuery();
                    if (INSID > 0)
                    {

                    }

                }
                return Json(new { status = "success", message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.ToString() });
            }
            // return Json(new { status = "success", });
        }


    }

}
