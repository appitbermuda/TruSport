using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisRanking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisPlayerSeasonID { get; set; }
        public string RankingTypeID { get; set; }
        public int Points { get; set; }

        [NotMapped]
        public int Rank { get; set; }

        public TennisPlayerSeason TennisPlayerSeason { get; set; }
        public RankingType RankingType { get; set; }
    }
}
