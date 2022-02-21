using System;
using SQLite;

namespace TruSport.Model
{
    public class Product
    {
        public string ID { get; set; }
        public string ProductTypeID { get; set; }
        public string TicketCompanyID { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public int MemberTicketCount { get; set; }

        //[Ignore]
        //public ProductType ProductType { get; set; }

        //[Ignore]
        //public TicketCompany TicketCompany { get; set; }
    }
}
