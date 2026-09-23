using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewShop.Models
{
    public class ProductPriceViewModel
    {
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public long Timestamp { get; set; }
        public string Signature { get; set; }
    }
}