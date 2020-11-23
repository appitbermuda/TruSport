using System;
namespace OnTrackWebService.Models
{
    public class PaymentResponse
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
    }
}
