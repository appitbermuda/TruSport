using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TeamSeason
    {
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

    }
}
