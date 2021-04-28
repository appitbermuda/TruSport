using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class TennisFixture
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string TournamentMatchTypeID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int NoOfSets { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }

        [Ignore]
        public DateTime FixtureTime => !String.IsNullOrEmpty(Time) ? TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : Date;

        [Ignore]
        public string SelectedPlayerID { get; set; }

        [Ignore]
        public string SelectedPlayerResult { get; set; }

        [Ignore]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [Ignore]
        public bool IsPostponedOrCancelled { get; set; }

        [Ignore]
        public TournamentMatchType TournamentMatchType { get; set; }

        [Ignore]
        public MatchType MatchType { get; set; }

        [Ignore]
        public ObservableCollection<TennisMatch> TennisMatch { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<TennisFixture> HeadToHead { get; set; }

        [Ignore]
        public string Player1Score { get; set; }

        [Ignore]
        public string Player2Score { get; set; }

        [Ignore]
        public string MatchResult { get; set; }
    }
}
