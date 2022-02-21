using System;
using SQLite;

namespace TruSport.Model
{
    public class BasketballLeagueStanding
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public int Played { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public decimal Percent { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public int PointsDifference { get; set; }

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
    }
}
