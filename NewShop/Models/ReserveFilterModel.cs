using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewShop.Models
{
    public class ReserveFilterModel
    {
        public string CUSCOD { get; set; }
        public string SLMCOD { get; set; }
        public string CUSNAM { get; set; }
        public string TELNUM { get; set; }
        public string CUSTYP { get; set; }
        public string CONTACT { get; set; }
        public string PRO { get; set; }
        public string Confirmed { get; set; }
        public string Actual { get; set; }
        public string GroupName { get; set; }      // คอลัมน์ DB ชื่อ "Group" (reserved word จึงใช้ GroupName ใน C#)
        public string Package { get; set; }
        public string LoyaltyGroup { get; set; }   // คอลัมน์ DB ชื่อ "Loyalty Group"
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string Reserve3 { get; set; }
        public string ReserveTB { get; set; }      // คอลัมน์ DB ชื่อ "Reserve TB"
        public string ConfirmDinner { get; set; }  // คอลัมน์ DB ชื่อ "Confirm_Dinner"

        // ===== เงื่อนไข "มากกว่า 0" ต่อคอลัมน์ =====
        // true = กรองเฉพาะแถวที่คอลัมน์นั้นมีค่า > 0, null/false = ไม่ใช้เงื่อนไขนี้
        public bool? ConfirmedGT0 { get; set; }
        public bool? ActualGT0 { get; set; }
        public bool? Reserve1GT0 { get; set; }
        public bool? Reserve2GT0 { get; set; }
        public bool? Reserve3GT0 { get; set; }
        public bool? ConfirmDinnerGT0 { get; set; }
    }
}