using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Order
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
