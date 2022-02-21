using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Basketball
{
    public class BasketballScore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BasketballFixtureID { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }

        [ForeignKey("BasketballFixtureID")]
        public BasketballFixture BasketballFixture { get; set; }
    }
}
