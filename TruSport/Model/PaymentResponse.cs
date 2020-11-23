using System;
namespace TruSport.Model
{
    public class PaymentResponse
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
    }
}
