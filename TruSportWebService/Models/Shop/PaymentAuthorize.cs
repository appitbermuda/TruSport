using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class PaymentAuthorize
    {
        public string CustomerID { get; set; }
        public string FixtureID { get; set; }
        public string SportEventID { get; set; }
        public string Email { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string Expiry { get; set; }
        public string Amount { get; set; }
        public int Quantity { get; set; }
        public bool IsWeb { get; set; }
        public bool SaveCard { get; set; }

        [NotMapped]
        public List<ContactTrace> ContactTraces { get; set; }

        [NotMapped]
        public List<OrderDetail> OrderDetails { get; set; }
    }    
}
