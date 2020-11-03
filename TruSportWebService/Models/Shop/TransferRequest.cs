using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OnTrackWebService.Models.Shop
{
    public class TransferRequest
    {
        public string MatchTicketID { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public MatchTicket MatchTicket { get; set; }
    }

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
    }
}
