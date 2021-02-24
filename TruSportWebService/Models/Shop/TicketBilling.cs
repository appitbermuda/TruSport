using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models.Shop
{
    public class TicketBilling
    {
        public string Fixture { get; set; }
        public double Quantity { get; set; }
        public double Fee { get; set; }
        public double Subtotal { get; set; }
        public double Total { get; set; }

        public List<Bill> Billing { get; set; }
    }

    public class Bill
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
        public double Total { get; set; }
    }
}
