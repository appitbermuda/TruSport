using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingTeamSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string LeagueID { get; set; }
        public string BowlingTeamID { get; set; }
        public string SeasonID { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [ForeignKey("BowlingTeamID")]
        public BowlingTeam BowlingTeam { get; set; }

        [NotMapped]
        public virtual List<BowlingFixture> BowlingFixtures { get; set; }

        [NotMapped]
        public virtual List<BowlingFixture> BowlingForm { get; set; }

        [NotMapped]
        public virtual List<PlayerSeason> Players { get; set; }

        [NotMapped]
        public virtual List<BowlingLeagueStanding> BowlingLeagueStandings { get; set; }
    }
}
