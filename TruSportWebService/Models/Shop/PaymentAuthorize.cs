using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class PaymentAuthorize
    {
        public string CustomerID { get; set; }
        public string FixtureProductID { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string Expiry { get; set; }
        public string Amount { get; set; }

        [NotMapped]
        public List<ContactTrace> ContactTraces { get; set; }
    }    
}
