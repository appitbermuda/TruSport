using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class WicketStats
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Team { get; set; }
        public int GamesPlayed { get; set; }
        public int RunsConceded { get; set; }
        public int Wickets { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
