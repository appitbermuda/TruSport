using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class TennisPlayerSeasons
    {
        public string PlayerID { get; set; }
        public string SeasonID { get; set; }
        public int MatchesPlayed { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
