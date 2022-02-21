using System;
using SQLite;

namespace TruSport.Model
{
    public class BasketballScore
    {
        public string ID { get; set; }
        public string BasketballFixtureID { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        [Ignore]
        public BasketballFixture BasketballFixture { get; set; }
    }
}
