using System;
using SQLite;

namespace TruSport.Model
{
    public class BowlingScore
    {
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public decimal? HomeTeamPoints { get; set; }
        public decimal? AwayTeamPoints { get; set; }
        public decimal? HomeTeamMatchPoints { get; set; }
        public decimal? AwayTeamMatchPoints { get; set; }

        [Ignore]
        public BowlingFixture BowlingFixture { get; set; }

        [Ignore]
        public decimal? HomeTeamTotalPoints { get; set; }

        [Ignore]
        public decimal? AwayTeamTotalPoints { get; set; }
    }
}
