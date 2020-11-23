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
}
