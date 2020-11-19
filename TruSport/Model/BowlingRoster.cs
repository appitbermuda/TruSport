using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class BowlingRoster
    {
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string TeamID { get; set; }
        public string BowlingPlayerSeasonID { get; set; }
        public int? Position { get; set; }

        [Ignore]
        public BowlingFixture BowlingFixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public BowlingPlayerSeason BowlingPlayerSeason { get; set; }

        [Ignore]
        public List<BowlingGame> BowlingGames { get; set; }

        [Ignore]
        public List<BowlingGameResult> BowlingGameResults { get; set; }
    }
}
