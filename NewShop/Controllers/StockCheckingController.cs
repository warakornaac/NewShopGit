using NewShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewShop.Controllers
{
    public class StockCheckingController : Controller
    {
        //
        // GET: /StockChecking/

        public ActionResult Index()
        {
            if (this.Session["UserType"] == "" || this.Session["UserType"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }
        public JsonResult GetCheckingStock(string Company, string Stkcod)
        {
            string Usr = "Thiraphon.pra";
            string message = string.Empty;
            List<StkCanSale> Getdata = new List<StkCanSale>();
            var connectionString = ConfigurationManager.ConnectionStrings["MobileOrder_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("P_Search_Stock_CanSales", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@inCompany", Company);
                cmd.Parameters.AddWithValue("@inSTKCOD", Stkcod);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Getdata.Add(new StkCanSale()
                    {
                        WH = !reader.IsDBNull(reader.GetOrdinal("WH")) ? reader.GetString(reader.GetOrdinal("WH")) : string.Empty,
                        Company = !reader.IsDBNull(reader.GetOrdinal("Company")) ? reader.GetString(reader.GetOrdinal("Company")) : string.Empty,
                        STKCOD = !reader.IsDBNull(reader.GetOrdinal("STKCOD")) ? reader.GetString(reader.GetOrdinal("STKCOD")) : string.Empty,
                        STKDES = !reader.IsDBNull(reader.GetOrdinal("STKDES")) ? reader.GetString(reader.GetOrdinal("STKDES")) : string.Empty,
                        STKGRP = !reader.IsDBNull(reader.GetOrdinal("STKGRP")) ? reader.GetString(reader.GetOrdinal("STKGRP")) : string.Empty,
                        UOM = !reader.IsDBNull(reader.GetOrdinal("UOM")) ? reader.GetString(reader.GetOrdinal("UOM")) : string.Empty,

                        ItemQty = !reader.IsDBNull(reader.GetOrdinal("ItemQty")) ? reader.GetDecimal(reader.GetOrdinal("ItemQty")).ToString("0") : string.Empty,
                        ReadyQty = !reader.IsDBNull(reader.GetOrdinal("ReadyQty")) ? reader.GetDecimal(reader.GetOrdinal("ReadyQty")).ToString("0") : string.Empty,
                        MO_Qty = !reader.IsDBNull(reader.GetOrdinal("Mobile_Qty")) ? reader.GetDecimal(reader.GetOrdinal("Mobile_Qty")).ToString("0") : string.Empty,
                        SO_Qty = !reader.IsDBNull(reader.GetOrdinal("SO_Qty")) ? reader.GetDecimal(reader.GetOrdinal("SO_Qty")).ToString("0") : string.Empty,
                        BuffQty = !reader.IsDBNull(reader.GetOrdinal("Buff_Qty")) ? reader.GetDecimal(reader.GetOrdinal("Buff_Qty")).ToString("0") : string.Empty,
                        RevQty = !reader.IsDBNull(reader.GetOrdinal("RevQty")) ? reader.GetDecimal(reader.GetOrdinal("RevQty")).ToString("0") : string.Empty,
                        InStock = !reader.IsDBNull(reader.GetOrdinal("InStock")) ? reader.GetString(reader.GetOrdinal("InStock")) : string.Empty,
                        BckDue = !reader.IsDBNull(reader.GetOrdinal("BckDue")) ? reader.GetDecimal(reader.GetOrdinal("BckDue")).ToString("0") : string.Empty

                    });
                }
                message = "Y";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { message = message, Getdata }, JsonRequestBehavior.AllowGet);
        }

    }
}
