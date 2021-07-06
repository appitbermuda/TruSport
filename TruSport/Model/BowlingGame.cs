using System;
using SQLite;

namespace TruSport.Model
{
    public class BowlingGame
    {
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string TeamID { get; set; }
        public string BowlingRosterID { get; set; }
        public int Score { get; set; }
        public int Game { get; set; }

        [Ignore]
        public BowlingFixture Fixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public BowlingRoster BowlingRoster { get; set; }
    }

    public class BowlingGameResult
    {
        public string ID { get; set; }
        public string BowlingRosterID1 { get; set; }
        public string BowlingRosterID2 { get; set; }
        public int? Score1 { get; set; }
        public int? Score2 { get; set; }
        public int Game { get; set; }
        public int? Position { get; set; }
        //public int? Points { get; set; }

        [Ignore]
        public BowlingRoster BowlingRoster1 { get; set; }

        [Ignore]
        public BowlingRoster BowlingRoster2 { get; set; }

        [Ignore]
        public string Winner { get; set; }
    }
}
