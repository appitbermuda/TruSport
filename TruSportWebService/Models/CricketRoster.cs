using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class CricketRoster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public int? JerseyNumber { get; set; }
        public bool IsReserve { get; set; }
        public bool IsColt { get; set; }

        [NotMapped]
        public bool IsHomeTeam { get; set; }

        [ForeignKey("FixtureID")]
        public CricketFixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        //[NotMapped]
        public virtual List<Batting> Batters { get; set; }

        //[NotMapped]
        public virtual List<Fielding> Fielders { get; set; }
    }
}
