using System;
using SQLite;

namespace TruSport.Model
{
    public class BowlingScore
    {
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public int? HomeTeamPoints { get; set; }
        public int? AwayTeamPoints { get; set; }
        public int? HomeTeamMatchPoints { get; set; }
        public int? AwayTeamMatchPoints { get; set; }

        [Ignore]
        public BowlingFixture BowlingFixture { get; set; }

        [Ignore]
        public int? HomeTeamTotalPoints { get; set; }

        [Ignore]
        public int? AwayTeamTotalPoints { get; set; }
    }
}
