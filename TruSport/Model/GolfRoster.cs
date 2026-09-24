using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class GolfRoster
    {
        public string ID { get; set; }
        public string GolfFixtureID { get; set; }
        public string LeagueID { get; set; }
        public string GolfPlayerSeasonID { get; set; }
        public int? Position { get; set; }

        [Ignore]
        public GolfScore GolfScore { get; set; }

        [Ignore]
        public GolfFixture GolfFixture { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public GolfPlayerSeason GolfPlayerSeason { get; set; }
    }
}
