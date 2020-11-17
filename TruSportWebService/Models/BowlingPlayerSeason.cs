using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingPlayerSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Pins { get; set; }
        public int? Average { get; set; }
        public decimal? PointsWon { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [ForeignKey("TeamID")]
        public BowlingTeam Team { get; set; }

    }

    public class BowlingPlayerSeasons
    {
        public int TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Games { get; set; }
        public int? Pins { get; set; }
        public int? Average { get; set; }
        public decimal? PointsWon { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
