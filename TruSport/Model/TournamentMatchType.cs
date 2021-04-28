using System;
namespace TruSport.Model
{
    public class TournamentMatchType
    {
        public string ID { get; set; }
        public string TennisTournamentID { get; set; }
        public string MatchTypeID { get; set; }

        public TennisTournament Tournament { get; set; }
        public MatchType MatchType { get; set; }
    }
}
