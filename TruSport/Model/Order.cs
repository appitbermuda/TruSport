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

        [Ignore]
        public Customer Customer { get; set; }

        [Ignore]
        public DateTime FixtureDate { get; set; }

        [Ignore]
        public string Fixture { get; set; }

        [Ignore]
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }

    public class CustomerOrder
    {
        public string OrderID { get; set; }
        public string FixtureID { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerID { get; set; }
        public string Product { get; set; }
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

        [Ignore]
        public string CustomerTicket { get; set; } 

        [Ignore]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(FixtureDate.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(FixtureDate.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(FixtureDate.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(FixtureDate.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);
    }
}
