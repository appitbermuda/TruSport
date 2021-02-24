using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class GoalStats
    {
        public string Team { get; set; }
        public string Name { get; set; }  
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int Goals { get; set; }
        public string League { get; set; }
        public string Season { get; set; }
        public int? GoalTime { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
