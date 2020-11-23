using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class MatchInning
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string BattingTeamID { get; set; }
        public string FieldingTeamID { get; set; }
        public int? Run { get; set; }
        public int? Wicket { get; set; }
        public decimal? Over { get; set; }
        public int? Order { get; set; }
        public int Inning { get; set; }

        [ForeignKey("FixtureID")]
        public CricketFixture Fixture { get; set; }

        [ForeignKey("BattingTeamID")]
        public Team BattingTeam { get; set; }

        [ForeignKey("FieldingTeamID")]
        public Team FieldingTeam { get; set; }
    }

}
