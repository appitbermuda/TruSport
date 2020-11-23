using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models
{
    public class BowlingFixtureListView
    {
        public List<BowlingFixture> PastFixtures { get; set; }
        public List<BowlingFixture> UpcomingFixtures { get; set; }
    }
}
