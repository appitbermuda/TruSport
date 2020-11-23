using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OnTrackWebService.Models
{
    public class BowlingTeam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public int TeamID { get; set; }
        public string LeagueID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }
        
        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [NotMapped]
        public virtual BowlingLeagueStanding BowlingLeagueStanding { get; set; }

        [NotMapped]
        public virtual List<BowlingTeamSeason> TeamSeasons { get; set; }

        [NotMapped]
        public virtual List<BowlingFixture> BowlingFixtures { get; set; }

        [NotMapped]
        public virtual List<BowlingFixture> BowlingForm { get; set; }

        [NotMapped]
        public virtual List<BowlingPlayerSeason> BowlingPlayers { get; set; }
    }

    public class BowlingTeams
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public string League { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
