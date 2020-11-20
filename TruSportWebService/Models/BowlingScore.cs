using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingScore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public int? HomeTeamPoints { get; set; }
        public int? AwayTeamPoints { get; set; }
        public int? HomeTeamMatchPoints { get; set; }
        public int? AwayTeamMatchPoints { get; set; }

        [ForeignKey("BowlingFixtureID")]
        public BowlingFixture BowlingFixture { get; set; }
    }
}
