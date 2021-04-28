using System;
namespace TruSport.Model
{
    public class TennisPlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string SeasonID { get; set; }
        public int MatchesPlayed { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public bool IsActive { get; set; }

        public Player Player { get; set; }
        public Season Season { get; set; }
    }
}
