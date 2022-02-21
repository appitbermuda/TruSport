using System;
using SQLite;

namespace TruSport.Model
{
    public class BasketballPlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Points { get; set; }
        public int? FieldGoal { get; set; }
        public int? ThreePoint { get; set; }
        public int? Rebound { get; set; }
        public bool IsActive { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public Team Team { get; set; }
    }
}
