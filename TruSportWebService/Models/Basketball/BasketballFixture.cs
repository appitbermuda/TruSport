using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Basketball
{
    public class BasketballFixture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }

        [ForeignKey("HomeTeamID")]
        public Team HomeTeam { get; set; }

        [ForeignKey("AwayTeamID")]
        public Team AwayTeam { get; set; }

        [ForeignKey("FieldID")]
        public Field Field { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [NotMapped]
        public string PostOrCanc => IsPostponed ? "Postponed" : IsCancelled ? "Cancelled" : String.Empty;

        [NotMapped]
        public bool IsPostponedOrCancelled { get; set; }

        [NotMapped]
        public int TicketAvailable { get; set; }

        [NotMapped]
        public virtual List<BasketballLeagueStanding> BasketballLeagueStanding { get; set; }

        public virtual BasketballScore Match { get; set; }

        [NotMapped]
        public List<BasketballFixture> HeadToHead { get; set; }

        //[NotMapped]
        public List<BasketballRoster> MatchRosters { get; set; }

        [NotMapped]
        public List<BasketballRosterListView> Rosters { get; set; }

        [NotMapped]
        public string SelectedTeamID { get; set; }

        [NotMapped]
        public string SelectedTeamResult { get; set; }

        //[NotMapped]
        //public virtual List<BasketballRosterSummary> MatchRosterSummary { get; set; }
    }
}
