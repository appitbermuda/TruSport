using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class TennisFixtures
    {
        public string Tournament { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public int? Player1Seed { get; set; }
        public int? Player2Seed { get; set; }
        public int Sets { get; set; }
        public string MatchType { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Season { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
