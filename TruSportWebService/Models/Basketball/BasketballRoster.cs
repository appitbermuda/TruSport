using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Basketball
{
    public class BasketballRoster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BasketballFixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string PositionID { get; set; }
        public int? JerseyNumber { get; set; }
        public int? Points { get; set; }
        public int? FieldGoal { get; set; }
        public int? ThreePoint { get; set; }
        public int? Rebound { get; set; }

        [NotMapped]
        public bool IsHomeTeam { get; set; }

        [ForeignKey("BasketballFixtureID")]
        public BasketballFixture BasketballFixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("PositionID")]
        public Position Position { get; set; }
    }

    public class BasketballRosterListView
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
