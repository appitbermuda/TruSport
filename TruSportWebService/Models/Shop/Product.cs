using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string ProductTypeID { get; set; }
        public string TeamID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("ProductTypeID")]
        public ProductType ProductType { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
