using System;
using SQLite;

namespace TruSport.Model
{
    public class BowlingLeagueStanding
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public decimal PointsWon { get; set; }
        public decimal PointsLost { get; set; }
        public int TeamAvg { get; set; }
        public int ScratchPins { get; set; }
        public int HighGame { get; set; }
        public int HighSers { get; set; }
        public int Week { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public int Position { get; set; }

        [Ignore]
        public bool IsSelectedTeam { get; set; }

        [Ignore]
        public string WeekUpdated { get; set; }
    }
}
