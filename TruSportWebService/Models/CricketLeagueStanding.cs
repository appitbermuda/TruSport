using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class CricketLeagueStanding
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int BattingPoints { get; set; }
        public int BowlingPoints { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

}
