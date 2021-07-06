using System;
using SQLite;

namespace TruSport.Model
{
    public class BowlingPlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Pins { get; set; }
        public int? Average { get; set; }
        public decimal? PointsWon { get; set; }
        public bool IsActive { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public Team Team { get; set; }
    }
}
