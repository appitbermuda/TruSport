using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public int Qty { get; set; }
        public decimal Subtotal { get; set; }

        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        [ForeignKey("FixtureProductID")]
        public FixtureProduct FixtureProduct { get; set; }
    }
}
