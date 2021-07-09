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
        public string TicketCompanyID { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? MemberPrice { get; set; }
        public decimal? Fee { get; set; }

        [ForeignKey("ProductTypeID")]
        public ProductType ProductType { get; set; }

        [ForeignKey("TicketCompanyID")]
        public TicketCompany TicketCompany { get; set; }


        //[ForeignKey("TeamID")]
        //public Team Team { get; set; }

        //[NotMapped]
        //public virtual Inventory Inventory { get; set; }
    }
}
