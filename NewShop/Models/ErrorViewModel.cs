using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewShop.Models
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public string PanelClass { get; set; }

        public string Icon { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public long ErrorId { get; set; }

        public bool IsAdmin { get; set; }

        public string ErrorType { get; set; }

        public string StackTrace { get; set; }
    }
}