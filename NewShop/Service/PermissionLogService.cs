using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Routing;

namespace NewShop.Service
{
    public class PermissionLogService
    {
        public static void LogDenied(HttpContextBase context, string permissionCode) {
            string connStr = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr)) {
                SqlCommand cmd = new SqlCommand("P_Insert_Permission_DeniedLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                RouteData route = context.Request.RequestContext.RouteData;
                cmd.Parameters.AddWithValue("@UserId", context.Session["UserID"] ?? "");
                cmd.Parameters.AddWithValue("@UserType", context.Session["UserType"] ?? "");
                cmd.Parameters.AddWithValue("@PermissionCode", permissionCode);
                cmd.Parameters.AddWithValue("@ControllerName", route.Values["controller"]?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@ActionName", route.Values["action"]?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@Url", context.Request.RawUrl);
                cmd.Parameters.AddWithValue("@HttpMethod", context.Request.HttpMethod);
                cmd.Parameters.AddWithValue("@IPAddress", context.Request.UserHostAddress);
                cmd.Parameters.AddWithValue("@Browser", context.Request.Browser.Browser);
                cmd.Parameters.AddWithValue("@SessionId", context.Session.SessionID);
                cmd.Parameters.AddWithValue("@UserAgent", context.Request.UserAgent ?? "");
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}