using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NewShop.Models;

namespace NewShop.Repository
{
    public class ProductRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        /// <summary>
        /// อ่านข้อมูลสินค้า
        /// </summary>
        public ProductModel GetProduct(int productId) {
            //ProductModel model = null;

            //using (SqlConnection conn = new SqlConnection(_connectionString)) {
            //    using (SqlCommand cmd = new SqlCommand("P_Get_Product", conn)) {
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.AddWithValue("@ProductId", productId);

            //        conn.Open();

            //        using (SqlDataReader dr =
            //            cmd.ExecuteReader()) {
            //            if (dr.Read()) {
            //                model = new ProductModel();

            //                model.ProductId = Convert.ToInt32(dr["ProductId"]);

            //                model.Price = Convert.ToDecimal(dr["Price"]);
            //            }
            //        }
            //    }
            //}
            ProductModel model = new ProductModel();
            model.ProductId = 1;
            model.Price = 99;
            model.OldPrice = 99;
            return model;

            //return model;
        }

        /// <summary>
        /// บันทึกราคา
        /// </summary>
        public void SavePrice(
            int productId,
            decimal price) {
            using (SqlConnection conn =
                new SqlConnection(_connectionString)) {
                using (SqlCommand cmd =
                    new SqlCommand("P_SavePrice", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    cmd.Parameters.AddWithValue("@Price", price);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}