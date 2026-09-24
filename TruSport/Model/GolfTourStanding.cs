using System;
using SQLite;

namespace TruSport.Model
{
    public class GolfTourStanding
    {
        public string ID { get; set; }
        public string GolfPlayerSeasonID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public int RoundsPlayed { get; set; }
        public decimal Points { get; set; }

        [Ignore]
        public GolfPlayerSeason GolfPlayer { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public int Position { get; set; }
    }
}
