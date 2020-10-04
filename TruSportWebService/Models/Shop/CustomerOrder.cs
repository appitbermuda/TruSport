using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
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
        public DateTime FixtureDate { get; set; }
        public string Time { get; set; }
        public string FieldName { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public bool Validated { get; set; }

        [NotMapped]
        public string ScannedTeamID { get; set; }

        [NotMapped]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? FixtureDate.AddDays(1).Add(TimeSpan.Parse(Time)) : FixtureDate.Add(TimeSpan.Parse(Time));

    }

    public class ScanCustomerOrder
    {
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime FixtureDate { get; set; }
        public string Time { get; set; }
        public string FieldName { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public bool Validated { get; set; }

        

        [NotMapped]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? FixtureDate.AddDays(1).Add(TimeSpan.Parse(Time)) : FixtureDate.Add(TimeSpan.Parse(Time));

    }
}
