using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class CricketScore
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string BattingTeamID { get; set; }
        public string BowlingTeamID { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public decimal? Overs { get; set; }
        public int? Extras { get; set; }
        public int? Bye { get; set; }
        public int? LegBye { get; set; }
        public int? NoBall { get; set; }
        public int? Wide { get; set; }
        public int Innings { get; set; }
        public int MatchInnings { get; set; }

        [ForeignKey("FixtureID")]
        public CricketFixture Fixture { get; set; }

        [ForeignKey("BattingTeamID")]
        public Team BattingTeam { get; set; }

        [ForeignKey("BowlingTeamID")]
        public Team BowlingTeam { get; set; }
    }

    public class CricketMatch
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public decimal? Overs { get; set; }
        public int? Extras { get; set; }
        public int? Bye { get; set; }
        public int? LegBye { get; set; }
        public int? NoBall { get; set; }
        public int? Wide { get; set; }

        [ForeignKey("FixtureID")]
        public CricketFixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }
    }
}
