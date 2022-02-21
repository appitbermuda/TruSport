using System;
using SQLite;

namespace TruSport.Model
{
    public class OrderDetail
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public string EventTicketID { get; set; }
        public int Qty { get; set; }
        public decimal Subtotal { get; set; }
        public bool IsMemberTicket { get; set; }

        [Ignore]
        public Order Order { get; set; }

        [Ignore]
        public EventTicket EventTicket { get; set; }

        [Ignore]
        public FixtureProduct FixtureProduct { get; set; }
    }
}
