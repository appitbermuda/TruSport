using System;
namespace TruSport.Model
{
    public class TennisGame
    {
        public string ID { get; set; }
        public string TennisSetID { get; set; }
        public int? Score { get; set; }
        public int? Game { get; set; }
        public int? Point { get; set; }

        public TennisSet TennisSet { get; set; }
    }
}
