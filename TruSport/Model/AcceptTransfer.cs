using System;
using Newtonsoft.Json;

namespace TruSport.Model
{
    public class AcceptTransfer
    {
        public string CustomerID { get; set; }
        public string MatchTicketID { get; set; }
        public string TransferCustomerID { get; set; }
        public bool Accept { get; set; }

        [JsonIgnore]
        public MatchTicket MatchTicket { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        [JsonIgnore]
        public Customer TransferCustomer { get; set; }
    }
}
