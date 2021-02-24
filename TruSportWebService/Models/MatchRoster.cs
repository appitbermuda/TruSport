using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class MatchRoster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string SubstitutePlayerID { get; set; }
        public int? JerseyNumber { get; set; }
        public bool IsStarter { get; set; }
        public int? SubstituteTime { get; set; }

        [NotMapped]
        public bool IsHomeTeam { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("SubstitutePlayerID")]
        public Player SubstitutePlayer { get; set; }

        public virtual List<MatchStat> MatchStats { get; set; }
    }

    public class RosterListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomePlayerID { get; set; }
        public string HomePlayerName { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayPlayerID { get; set; }
        public string AwayPlayerName { get; set; }
        public int? HomeJerseyNumber { get; set; }
        public int? AwayJerseyNumber { get; set; }
        public bool IsStarter { get; set; }
        public bool IsHomeTeam { get; set; }
    }
}
