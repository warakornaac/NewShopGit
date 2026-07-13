using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NewShop.Service
{
    public class PermissionService
    {
        public static List<string> GetPermissions(string userType) {
            List<string> permissions = new List<string>();

            string connStr = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr)) {
                SqlCommand cmd = new SqlCommand("P_Get_Permissions_By_UserType", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserType", userType);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    permissions.Add(
                        reader["PermissionCode"].ToString()
                    );
                }
            }

            return permissions;
        }
    }
}