using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NewShop.Models;

namespace NewShop.Service
{
    public class ErrorLogService
    {
        public static long Log(Exception ex) {
            long errorId = 0;

            string connStr = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr)) {
                SqlCommand cmd = new SqlCommand("P_Insert_ErrorLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Current.Session?["UserID"] ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UserType", HttpContext.Current.Session?["UserType"] ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Url", HttpContext.Current.Request.RawUrl);
                cmd.Parameters.AddWithValue("@HttpMethod", HttpContext.Current.Request.HttpMethod);
                cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);
                cmd.Parameters.AddWithValue("@Browser", HttpContext.Current.Request.Browser.Browser);
                cmd.Parameters.AddWithValue("@ErrorType", ex.GetType().Name);
                cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                cmd.Parameters.AddWithValue("@StackTrace", ex.ToString());
                cmd.Parameters.AddWithValue("@InnerException", ex.InnerException?.ToString() ?? "");
                conn.Open();
                errorId = Convert.ToInt64(cmd.ExecuteScalar());
            }

            return errorId;
        }
        public static ErrorViewModel GetError(long errorId) {
            ErrorViewModel model = null;
            string connStr = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr)) {
                SqlCommand cmd = new SqlCommand("P_Get_ErrorLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ErrorId", errorId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    model = new ErrorViewModel {
                        ErrorId = Convert.ToInt64(reader["ErrorId"]),
                        //ErrorCode = "500",
                        Message = reader["ErrorMessage"].ToString(),
                        ErrorType = reader["ErrorType"].ToString(),
                        StackTrace = reader["StackTrace"].ToString()
                    };
                }
            }
            return model;
        }
    }
}