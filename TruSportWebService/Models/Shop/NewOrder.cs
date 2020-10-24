using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class NewOrder
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
