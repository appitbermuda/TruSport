using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingRoster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string TeamID { get; set; }
        public string BowlingPlayerSeasonID { get; set; }
        public int? Position { get; set; }

        [ForeignKey("BowlingFixtureID")]
        public BowlingFixture BowlingFixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("BowlingPlayerSeason")]
        public BowlingPlayerSeason BowlingPlayerSeason { get; set; }
    }
}
