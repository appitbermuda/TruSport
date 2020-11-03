using System;
namespace TruSport.Model
{
    public class AcceptTransfer
    {
        public string CustomerID { get; set; }
        public string MatchTicketID { get; set; }
        public string TransferCustomerID { get; set; }
        public bool Accept { get; set; }
    }
}
