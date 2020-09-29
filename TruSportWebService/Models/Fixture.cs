using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Fixture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public string SportID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsPostponed { get; set; }
        //public bool IsCancelled { get; set; }

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

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }

        [NotMapped]
        //public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;
        public string PostOrCanc => IsPostponed ? "Post." : String.Empty;

        [NotMapped]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? Date.AddDays(1).Add(TimeSpan.Parse(Time)) : Date.Add(TimeSpan.Parse(Time));

        [NotMapped]
        public string SelectedTeamID { get; set; }

        [NotMapped]
        public string SelectedTeamResult { get; set; }

        [NotMapped]
        public virtual List<Fixture> Form { get; set; }

        //[NotMapped]
        public virtual Match Match { get; set; }

        [NotMapped]
        public virtual List<LeagueTable> LeagueTable { get; set; }

        [NotMapped]
        public virtual List<Fixture> HeadToHead { get; set; }

        [NotMapped]
        public virtual List<MatchRoster> MatchRosters { get; set; }

        [NotMapped]
        public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }
}
