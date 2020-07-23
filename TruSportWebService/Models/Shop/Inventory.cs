using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Inventory
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public int Stock { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
