using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingFixture
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
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        [ForeignKey("HomeTeamID")]
        public BowlingTeam HomeTeam { get; set; }

        [ForeignKey("AwayTeamID")]
        public BowlingTeam AwayTeam { get; set; }

        [ForeignKey("FieldID")]
        public Field Field { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [NotMapped]
        public bool IsPostponedOrCancelled => IsPostponed || IsCancelled;

        [NotMapped]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? Date.AddDays(1).Add(TimeSpan.Parse(Time)) : Date.Add(TimeSpan.Parse(Time));

        [NotMapped]
        public string SelectedTeamID { get; set; }

        [NotMapped]
        public string SelectedTeamResult { get; set; }

        [NotMapped]
        public string HomeTeamScore { get; set; }

        [NotMapped]
        public string AwayTeamScore { get; set; }

        [NotMapped]
        public string MatchResult { get; set; }

        [NotMapped]
        public virtual List<BowlingLeagueStanding> LeagueTable { get; set; }

        [NotMapped]
        public virtual List<BowlingFixture> HeadToHead { get; set; }

        //public virtual List<MatchInning> MatchInnings { get; set; }

        [NotMapped]
        public virtual List<BowlingGame> BowlingGame { get; set; }

        public virtual List<BowlingRoster> BowlingRosters { get; set; }

        //[NotMapped]
        //public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }

    public class BowlingFixtures
    {
        public int HomeTeamID { get; set; }
        public int AwayTeamID { get; set; }
        public string Field { get; set; }
        public string League { get; set; }
        public string MatchType { get; set; }
        public DateTime Date { get; set; }
        //public DateTime Time { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
