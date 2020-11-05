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

        [NotMapped]
        public MatchTicket MatchTicket { get; set; }
    }

    public class AcceptTransfer
    {
        public string CustomerID { get; set; }
        public string MatchTicketID { get; set; }
        public string TransferCustomerID { get; set; }
        public bool Accept { get; set; }

        [NotMapped]
        public MatchTicket MatchTicket { get; set; }

        [NotMapped]
        public Customer Customer { get; set; }

        [NotMapped]
        public Customer TransferCustomer { get; set; }
    }
}
