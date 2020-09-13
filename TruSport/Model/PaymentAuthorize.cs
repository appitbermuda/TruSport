using System;
namespace TruSport.Model
{
    public class PaymentAuthorize
    {
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string Expiry { get; set; }
        public string Amount { get; set; }
    }
}
