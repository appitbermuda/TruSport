using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class BasketballRoster
    {
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
        public bool IsStarter { get; set; }
        public bool IsSub { get; set; }

        [Ignore]
        public bool IsHomeTeam { get; set; }

        [Ignore]
        public BasketballFixture BasketballFixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }
    }
}
