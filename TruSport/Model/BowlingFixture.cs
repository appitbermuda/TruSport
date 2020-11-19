using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class BowlingFixture
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
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        [Ignore]
        public string SelectedTeamID { get; set; }

        [Ignore]
        public string SelectedTeamResult { get; set; }

        [Ignore]
        public DateTime FixtureTime => !String.IsNullOrEmpty(Time) ? TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : Date;

        [Ignore]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [Ignore]
        public bool IsPostponedOrCancelled { get; set; }

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

        //[Ignore]
        //public BowlingMatch BowlingMatch { get; set; }

        [Ignore]
        public virtual List<BowlingLeagueStanding> LeagueTable { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<BowlingFixture> HeadToHead { get; set; }

        [Ignore]
        public ObservableCollection<BowlingRoster> BowlingRosters { get; set; }

        [Ignore]
        public ObservableCollection<BowlingGame> BowlingGames { get; set; }

        [Ignore]
        public virtual List<BowlingGameResult> BowlingGameResults { get; set; }

        [Ignore]
        public virtual List<BowlingRosterListView> BowlingRosterList { get; set; }

        [Ignore]
        public string HomeTeamScore { get; set; }

        [Ignore]
        public string AwayTeamScore { get; set; }

        [Ignore]
        public string MatchResult { get; set; }

        //[Ignore]
        //public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }
}
