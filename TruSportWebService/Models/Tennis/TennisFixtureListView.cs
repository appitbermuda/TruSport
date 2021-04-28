using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models
{
    public class TennisFixtureListView
    {
        public List<TennisFixture> PastFixtures { get; set; }
        public List<TennisFixture> UpcomingFixtures { get; set; }
    }
}
