using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewShop.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }

        /// <summary>
        /// HMAC Signature
        /// </summary>
        public string Signature { get; set; }
    }
}