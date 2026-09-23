using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NewShop.Models;

namespace NewShop.Repository
{
    public class ReserveRepository
    {
        private readonly string _connStr =
            ConfigurationManager.ConnectionStrings["Promotion_ConnectionString"].ConnectionString;

        public List<ReserveViewModel> Search(ReserveFilterModel f) {
            var result = new List<ReserveViewModel>();

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("P_Search_Cuseven", conn)) {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;

                AddParam(cmd, "@CUSCOD", f.CUSCOD);
                AddParam(cmd, "@SLMCOD", f.SLMCOD);
                AddParam(cmd, "@CUSNAM", f.CUSNAM);
                AddParam(cmd, "@TELNUM", f.TELNUM);
                AddParam(cmd, "@CUSTYP", f.CUSTYP);
                AddParam(cmd, "@CONTACT", f.CONTACT);
                AddParam(cmd, "@PRO", f.PRO);
                AddParam(cmd, "@Confirmed", f.Confirmed);
                AddParam(cmd, "@Actual", f.Actual);
                AddParam(cmd, "@GroupName", f.GroupName);
                AddParam(cmd, "@Package", f.Package);
                AddParam(cmd, "@LoyaltyGroup", f.LoyaltyGroup);
                AddParam(cmd, "@Reserve1", f.Reserve1);
                AddParam(cmd, "@Reserve2", f.Reserve2);
                AddParam(cmd, "@Reserve3", f.Reserve3);
                AddParam(cmd, "@ReserveTB", f.ReserveTB);
                AddParam(cmd, "@ConfirmDinner", f.ConfirmDinner);

                // ===== เงื่อนไข "มากกว่า 0" =====
                AddBitParam(cmd, "@ConfirmedGT0", f.ConfirmedGT0);
                AddBitParam(cmd, "@ActualGT0", f.ActualGT0);
                AddBitParam(cmd, "@Reserve1GT0", f.Reserve1GT0);
                AddBitParam(cmd, "@Reserve2GT0", f.Reserve2GT0);
                AddBitParam(cmd, "@Reserve3GT0", f.Reserve3GT0);
                AddBitParam(cmd, "@ConfirmDinnerGT0", f.ConfirmDinnerGT0);

                conn.Open();
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new ReserveViewModel {
                            Event = reader["Event"] as string,
                            CUSCOD = reader["CUSCOD"] as string,
                            SLMCOD = reader["SLMCOD"] as string,
                            CUSNAM = reader["CUSNAM"] as string,
                            TELNUM = reader["TELNUM"] as string,
                            CUSTYP = reader["CUSTYP"] as string,
                            CONTACT = reader["CONTACT"] as string,
                            PRO = reader["PRO"] as string,
                            Confirmed = ToNullableInt(reader["Confirmed"]),
                            Actual = ToNullableInt(reader["Actual"]),
                            GroupName = reader["Group"] as string,
                            Package = reader["Package"] as string,
                            LoyaltyGroup = reader["Loyalty Group"] as string,
                            Reserve1 = reader["Reserve1"] as string,
                            Reserve2 = reader["Reserve2"] as string,
                            Reserve3 = reader["Reserve3"] as string,
                            ReserveTB = reader["Reserve TB"] as string,
                            ConfirmDinner = ToNullableInt(reader["Confirm_Dinner"])
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Helper: ส่ง DBNull เมื่อ string ว่าง เพื่อให้ SP มองเป็น "ไม่กรองคอลัมน์นี้"
        /// </summary>
        private void AddParam(SqlCommand cmd, string name, string value) {
            cmd.Parameters.Add(name, SqlDbType.NVarChar, 200).Value =
                string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        /// <summary>
        /// Helper: ส่งค่า BIT สำหรับเงื่อนไข "มากกว่า 0"
        /// value = null (checkbox ไม่ได้ติ๊ก) -> ส่ง DBNull -> SP มองเป็น "ไม่ใช้เงื่อนไขนี้"
        /// </summary>
        private void AddBitParam(SqlCommand cmd, string name, bool? value) {
            cmd.Parameters.Add(name, SqlDbType.Bit).Value =
                value.HasValue ? (object)value.Value : DBNull.Value;
        }

        /// <summary>
        /// แปลงค่าจาก DB เป็น int? อย่างปลอดภัย รองรับ int, decimal, numeric
        /// ใช้กับคอลัมน์ที่ยืนยันแล้วว่าเป็นชนิดตัวเลขจริง (Confirmed, Actual, Confirm_Dinner)
        /// </summary>
        private int? ToNullableInt(object value) {
            if (value == null || value == DBNull.Value)
                return null;

            try {
                return Convert.ToInt32(value);
            }
            catch {
                return null;
            }
        }
    }
}