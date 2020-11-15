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

        [ForeignKey("BowlingFixtureID")]
        public BowlingFixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public BowlingTeam Team { get; set; }

        [ForeignKey("BowlingRosterID")]
        public BowlingRoster BowlingRoster { get; set; }

    }

    public class BowlingGames
    {
        public int TeamID { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Score { get; set; }
        public int Game { get; set; }
        public decimal Points { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
