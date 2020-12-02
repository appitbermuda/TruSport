using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingScore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public decimal? HomeTeamPoints { get; set; }
        public decimal? AwayTeamPoints { get; set; }
        public decimal? HomeTeamMatchPoints { get; set; }
        public decimal? AwayTeamMatchPoints { get; set; }

        [ForeignKey("BowlingFixtureID")]
        public BowlingFixture BowlingFixture { get; set; }

        [NotMapped]
        public decimal? HomeTeamTotalPoints => HomeTeamMatchPoints + HomeTeamPoints;

        [NotMapped]
        public decimal? AwayTeamTotalPoints => AwayTeamMatchPoints + AwayTeamPoints;
    }
}
