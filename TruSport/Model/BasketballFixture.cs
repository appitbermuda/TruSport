using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class BasketballFixture
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string GameTime { get; set; }
        public string SelectedTeamID { get; set; }
        public string SelectedTeamResult { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
        
        [Ignore]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [Ignore]
        public string PostOrCanc => IsPostponed ? "Postponed" : IsCancelled ? "Cancelled" : String.Empty;

        [Ignore]
        public bool IsPostponedOrCancelled { get; set; }

        [Ignore]
        public int TicketAvailable { get; set; }

        [Ignore]
        public Team HomeTeam { get; set; }

        [Ignore]
        public Team AwayTeam { get; set; }

        [Ignore]
        public Field Field { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public MatchType MatchType { get; set; }

        [Ignore]
        public BasketballScore Match { get; set; }

        [Ignore]
        public virtual List<BasketballLeagueStanding> BasketballLeagueStanding { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<BasketballFixture> HeadToHead { get; set; }

        [Ignore]
        public ObservableCollection<BasketballRoster> Rosters { get; set; }

        //[Ignore]
        //public ObservableCollection<BasketballRosterListView> Rosters { get; set; }

        //[Ignore]
        //public virtual List<BasketballRosterSummary> MatchRosterSummary { get; set; }
    }
}
