using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class RunStats
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Team { get; set; }
        public int GamesPlayed { get; set; }
        public int BallsFaced { get; set; }
        public int Runs { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
