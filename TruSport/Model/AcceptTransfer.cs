using System;
using Newtonsoft.Json;
using SQLite;

namespace TruSport.Model
{
    public class AcceptTransfer
    {
        public string CustomerID { get; set; }
        public string MatchTicketID { get; set; }
        public string TransferCustomerID { get; set; }
        public bool Accept { get; set; }

        [Ignore]
        public MatchTicket MatchTicket { get; set; }

        [Ignore]
        public Customer Customer { get; set; }

        [Ignore]
        public Customer TransferCustomer { get; set; }
    }
}
