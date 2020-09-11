using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Reports.Models
{
    public class InvoiceViewModel
    {
        public Master Master { get; set; }

        public List<Detail> Details { get; set; }

        public decimal Total { get; set; }
    }
}