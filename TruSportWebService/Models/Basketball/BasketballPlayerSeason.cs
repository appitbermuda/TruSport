using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Basketball
{
    public class BasketballPlayerSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Points { get; set; }
        public int? FieldGoal { get; set; }
        public int? ThreePoint { get; set; }
        public int? Rebound { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }
    }
}
