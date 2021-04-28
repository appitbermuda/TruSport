using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisFixture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TournamentMatchTypeID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int NoOfSets { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }

        [ForeignKey("TournamentMatchTypeID")]
        public TournamentMatchType TournamentMatchType { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public DateTime FixtureTime => !String.IsNullOrEmpty(Time) ? TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : Date;

        [NotMapped]
        public string SelectedPlayerID { get; set; }

        [NotMapped]
        public string SelectedPlayerResult { get; set; }

        [NotMapped]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [NotMapped]
        public bool IsPostponedOrCancelled { get; set; }

        //[NotMapped]
        public virtual List<TennisMatch> TennisMatch { get; set; }


        [NotMapped]
        public List<TennisFixture> HeadToHead { get; set; }

        [NotMapped]
        public int Player1Score { get; set; }

        [NotMapped]
        public int Player2Score { get; set; }

        [NotMapped]
        public string MatchResult { get; set; }
    }
}
