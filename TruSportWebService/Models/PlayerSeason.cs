using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class PlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Goals { get; set; }
        public int? YellowCards { get; set; }
        public int? RedCards { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

    }
}
