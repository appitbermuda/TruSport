using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OnTrackWebService.Models.Basketball;

namespace OnTrackWebService.Models
{
    public class TeamSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string LeagueID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [NotMapped]
        public virtual List<Transfer> Transfers { get; set; }

        [NotMapped]
        public virtual List<BasketballFixture> BasketballFixtures { get; set; }

        [NotMapped]
        public virtual List<BasketballFixture> BasketballForm { get; set; }

        [NotMapped]
        public virtual List<BasketballLeagueStanding> BasketballTable { get; set; }

        [NotMapped]
        public virtual List<CricketFixture> CricketFixtures { get; set; }

        [NotMapped]
        public virtual List<CricketFixture> CricketForm { get; set; }

        [NotMapped]
        public virtual List<Fixture> Fixtures { get; set; }

        [NotMapped]
        public virtual List<Fixture> Form { get; set; }

        [NotMapped]
        public virtual List<PlayerSeason> Players { get; set; }

        [NotMapped]
        public virtual List<LeagueTable> FootballTable { get; set; }

        [NotMapped]
        public virtual List<CricketLeagueTable> CricketTable { get; set; }
    }
}
