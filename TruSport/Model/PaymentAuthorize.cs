using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class PaymentAuthorize
    {
        public string CustomerID { get; set; }
        public string FixtureProductID { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string Expiry { get; set; }
        public string Amount { get; set; }
        public int Quantity { get; set; }

        [Ignore]
        public List<ContactTrace> ContactTraces { get; set; }
    }
}
