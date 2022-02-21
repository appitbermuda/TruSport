using System;
using SQLite;

namespace TruSport.Model.Ticket
{
    public class Wallet
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string TokenPAN { get; set; }
        public string Expiry { get; set; }
        public bool IsDefault { get; set; }

        [Ignore]
        public Customer Customer { get; set; }
    }
}
