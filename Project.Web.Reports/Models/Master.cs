using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Reports.Models
{
    public class Master
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime DueDate { get; set; }

        public Company Company { get; set; }

        public Client Client { get; set; }

        public string PaymentMethod { get; set; }

        public string Amount { get; set; }
    }
}