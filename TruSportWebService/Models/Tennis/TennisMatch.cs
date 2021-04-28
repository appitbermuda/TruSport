using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisMatch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisFixtureID { get; set; }
        public string Player1ID { get; set; }
        public string Player2ID { get; set; }
        public int? Seed { get; set; }

        [ForeignKey("TennisFixtureID")]
        public TennisFixture TennisFixture { get; set; }

        [ForeignKey("Player1ID")]
        public TennisPlayerSeason Player1 { get; set; }

        [ForeignKey("Player2ID")]
        public TennisPlayerSeason Player2 { get; set; }

        [NotMapped]
        public int? Set1 { get; set; }

        [NotMapped]
        public int? Set2 { get; set; }

        [NotMapped]
        public int? Set3 { get; set; }

        [NotMapped]
        public int? Set4 { get; set; }

        [NotMapped]
        public int? Set5 { get; set; }

        [NotMapped]
        public int? TiebreakSet1 { get; set; }

        [NotMapped]
        public int? TiebreakSet2 { get; set; }

        [NotMapped]
        public int? TiebreakSet3 { get; set; }

        [NotMapped]
        public int? TiebreakSet4 { get; set; }

        [NotMapped]
        public int? TiebreakSet5 { get; set; }

        //[NotMapped]
        public virtual ObservableCollection<TennisSet> TennisSets { get; set; }
    }
}
