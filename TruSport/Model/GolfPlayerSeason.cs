using System;
using SQLite;

namespace TruSport.Model
{
    public class GolfPlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string LeagueID { get; set; }
        public string SeasonID { get; set; }
        public int? RoundsPlayed { get; set; }
        public int? Score { get; set; }
        public decimal? Points { get; set; }
        public bool IsActive { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public League League { get; set; }
    }
}
