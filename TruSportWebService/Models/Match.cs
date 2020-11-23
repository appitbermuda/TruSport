using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Match
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? HomeYellowCards { get; set; }
        public int? AwayYellowCards { get; set; }
        public int? HomeRedCards { get; set; }
        public int? AwayRedCards { get; set; }
        public bool? IsPenalties { get; set; }
        public int? HomeTeamPenalty { get; set; }
        public int? AwayTeamPenalty { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

    }
}
