using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class CricketFixture
    {
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        //public string CompetitionID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

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

        //[ForeignKey("CompetitionID")]
        //public Competition Competition { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

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

        //public virtual CricketMatch CricketMatch { get; set; }

        //[NotMapped]
        //public virtual List<CricketFixture> Form { get; set; }

        [NotMapped]
        public virtual List<CricketLeagueTable> LeagueTable { get; set; }

        [NotMapped]
        public virtual List<CricketFixture> HeadToHead { get; set; }

        public virtual List<MatchInning> MatchInnings { get; set; }

        [NotMapped]
        public virtual List<CricketMatch> CricketMatch { get; set; }

        public virtual List<CricketRoster> CricketRosters { get; set; }

        //[NotMapped]
        //public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }
}
