using System;
using System.Collections.Generic;

namespace TruSport.Model
{
    public class GolfFixtureListView
    {
        public List<GolfFixture> PastFixtures { get; set; }
        public List<GolfFixture> UpcomingFixtures { get; set; }
    }
}
