using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class TennisMatch
    {
        public string ID { get; set; }
        public string TennisFixtureID { get; set; }
        public string Player1ID { get; set; }
        public string Player2ID { get; set; }
        public int? Seed { get; set; }

        public TennisFixture TennisFixture { get; set; }
        public TennisPlayerSeason Player1 { get; set; }
        public TennisPlayerSeason Player2 { get; set; }

        [Ignore]
        public int? Set1 { get; set; }

        [Ignore]
        public int? Set2 { get; set; }

        [Ignore]
        public int? Set3 { get; set; }

        [Ignore]
        public int? Set4 { get; set; }

        [Ignore]
        public int? Set5 { get; set; }

        [Ignore]
        public int? TiebreakSet1 { get; set; }

        [Ignore]
        public int? TiebreakSet2 { get; set; }

        [Ignore]
        public int? TiebreakSet3 { get; set; }

        [Ignore]
        public int? TiebreakSet4 { get; set; }

        [Ignore]
        public int? TiebreakSet5 { get; set; }

        [Ignore]
        public virtual ObservableCollection<TennisSet> TennisSets { get; set; }
    }
}
