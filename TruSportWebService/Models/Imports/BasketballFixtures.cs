using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class BasketballFixtures
    {
        public string Home { get; set; }
        public string Away { get; set; }
        public string Field { get; set; }
        public string League { get; set; }
        public string MatchType { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
