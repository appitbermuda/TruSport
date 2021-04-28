using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisPlayerSeason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string SeasonID { get; set; }
        public int MatchesPlayed { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public bool IsActive { get; set; }

        public Player Player { get; set; }
        public Season Season { get; set; }
    }
}
