using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class Order
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Authorisation { get; set; }

        [Ignore]
        public Customer Customer { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
