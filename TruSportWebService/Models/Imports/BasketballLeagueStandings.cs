using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class BasketballLeagueStandings
    {
        public string Team { get; set; }
        public string League { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public int Points { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public int PointsDifference { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
