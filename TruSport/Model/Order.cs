using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class Order
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Authorisation { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }

        [Ignore]
        public Customer Customer { get; set; }

        [Ignore]
        public virtual OrderDetail OrderDetail { get; set; }
    }

    public class CustomerOrder
    {
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FixtureDate { get; set; }
        public string FixtureTime { get; set; }
        public string FieldName { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
    }
}
