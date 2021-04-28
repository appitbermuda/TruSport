using System;
namespace TruSport.Model
{
    public class TennisRanking
    {
        public string ID { get; set; }
        public int Rank { get; set; }
        public string TennisPlayerSeasonID { get; set; }
        public string RankingTypeID { get; set; }
        public int Points { get; set; }

        public TennisPlayerSeason TennisPlayerSeason { get; set; }
        public RankingType RankingType { get; set; }
    }
}
