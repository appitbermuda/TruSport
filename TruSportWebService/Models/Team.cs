using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OnTrackWebService.Models
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string HomeFieldID { get; set; }
        public string LeagueID { get; set; }
        public string SportID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }
        

        [ForeignKey("HomeFieldID")]
        public Field Field { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }

        [NotMapped]
        public virtual LeagueTable FootballTable { get; set; }

        [NotMapped]
        public virtual CricketLeagueTable CricketTable { get; set; }

        [NotMapped]
        public virtual List<TeamSeason> TeamSeasons { get; set; }

        [NotMapped]
        public virtual List<Coach> Coaches { get; set; }

        [NotMapped]
        public League League { get; set; }

        [NotMapped]
        public virtual List<CricketFixture> CricketFixtures { get; set; }

        [NotMapped]
        public virtual List<Fixture> Fixtures { get; set; }

        //[NotMapped]
        //public virtual List<Transfer> Transfers { get; set; }

        [NotMapped]
        public virtual List<CricketFixture> CricketForm { get; set; }

        [NotMapped]
        public virtual List<Fixture> Form { get; set; }

        //[NotMapped]
        //public virtual List<PlayerSeason> Players { get; set; }

        //[NotMapped]
        //public virtual List<LeagueTable> FootballTable { get; set; }

        //[NotMapped]
        //public virtual List<CricketLeagueTable> CricketTable { get; set; }
    }
}
