using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewShop.Models
{
    public class ReserveViewModel
    {
        public string Event { get; set; }
        public string CUSCOD { get; set; }
        public string SLMCOD { get; set; }
        public string CUSNAM { get; set; }
        public string TELNUM { get; set; }
        public string CUSTYP { get; set; }
        public string CONTACT { get; set; }
        public string PRO { get; set; }
        public int? Confirmed { get; set; }
        public int? Actual { get; set; }
        public string GroupName { get; set; }
        public string Package { get; set; }
        public string LoyaltyGroup { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string Reserve3 { get; set; }
        public string ReserveTB { get; set; }
        public int? ConfirmDinner { get; set; }
    }
}