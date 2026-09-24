using System;
using SQLite;

namespace TruSport.Model
{
    public class GolfScore
    {
        public string ID { get; set; }
        public string GolfFixtureID { get; set; }
        public string GolfRosterID { get; set; }
        public decimal? Score { get; set; }
        public decimal? Points { get; set; }

        [Ignore]
        public GolfRoster GolfRoster { get; set; }

        [Ignore]
        public GolfFixture GolfFixture { get; set; }
    }
}
