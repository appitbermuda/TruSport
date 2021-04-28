using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class TennisRankings
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Points { get; set; }
        public string RankingTypeID { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
