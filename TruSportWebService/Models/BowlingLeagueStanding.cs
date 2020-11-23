using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingLeagueStanding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public decimal PointsWon { get; set; }
        public decimal PointsLost { get; set; }
        public int TeamAvg { get; set; }
        public int ScratchPins { get; set; }
        public int HighGame { get; set; }
        public int HighSers { get; set; }

        [ForeignKey("TeamID")]
        public BowlingTeam Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public int Position { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }
    }

}
