using System;
namespace OnTrackWebService.Models.Shop
{
    public class TicketBilling
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
        public double Total { get; set; }
    }
}
