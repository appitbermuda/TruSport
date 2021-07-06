using System;
using System.Collections.Generic;

namespace TruSport.Model
{
    public class BowlingFixtureListView
    {
        public List<BowlingFixture> PastFixtures { get; set; }
        public List<BowlingFixture> UpcomingFixtures { get; set; }
    }
}
