using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Basketball
{
    public class BasketballLeagueStanding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string LeagueID { get; set; }
        public int Played { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        [NotMapped]
        public int Position { get; set; }

        [NotMapped]
        public int PointsDifference => PointsFor - PointsAgainst;

        [NotMapped]
        public decimal Percent => (Win + Loss) > 0 ? ((1 / (Win + Loss)) * Win) : 0.0m;

        [NotMapped]
        public bool IsSelectedTeam { get; set; }
    }
}
