using System;
using SQLite;

namespace TruSport.Model
{
    public class OrderDetail
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public int Qty { get; set; }
        public decimal Subtotal { get; set; }

        [Ignore]
        public Order Order { get; set; }

        [Ignore]
        public Product Product { get; set; }
    }
}
