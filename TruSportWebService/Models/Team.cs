using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Team
    {
        public string ID { get; set; }
        public string HomeFieldID { get; set; }
        public string LeagueID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }
        

        [ForeignKey("HomeFieldID")]
        public Field Field { get; set; }

        //public virtual List<TeamSeason> Teams { get; set; }
        //public virtual List<PlayerSeason> Players { get; set; }
        public virtual List<Coach> Coaches { get; set; }
    }
}
