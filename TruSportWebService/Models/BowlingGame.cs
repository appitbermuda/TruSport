using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingGame
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string TeamID { get; set; }
        public string BowlingRosterID { get; set; }
        public int Score { get; set; }
        public int Game { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("BowlingRosterID")]
        public BowlingRoster BowlingRoster { get; set; }

    }
}
