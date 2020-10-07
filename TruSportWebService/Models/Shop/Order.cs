using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Authorisation { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        [NotMapped]
        public DateTime FixtureDate { get; set; }

        [NotMapped]
        public string Fixture { get; set; }

        [NotMapped]
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
