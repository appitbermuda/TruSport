using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class SportEvent
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Season { get; set; }
        public string SportID { get; set; }
        public string FieldID { get; set; }
        public string TicketCompanyID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string EventLogo { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
        public string Term { get; set; }
        public string DefaultTicketTerm { get; set; }
        public string DefaultContactTraceTerm { get; set; }

        [Ignore]
        public DateTime EventTime => !String.IsNullOrEmpty(Time) ? TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : Date;

        [Ignore]
        public string PostponedOrCancelled => IsPostponed ? "Postponed" : IsCancelled ? "Cancelled" : String.Empty;


        [Ignore]
        public Sport Sport { get; set; }

        [Ignore]
        public Field Field { get; set; }

        [Ignore]
        public TicketCompany TicketCompany { get; set; }
    }
}
