using System;
namespace TruSport.Model
{
    public class TennisTiebreak
    {
        public string ID { get; set; }
        public string TennisSetID { get; set; }
        public int? Score { get; set; }

        public TennisSet Set { get; set; }
    }
}
