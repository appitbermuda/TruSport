using System;
namespace TruSport.Model
{
    public class LeagueStat
    {
        public string TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public int Goals { get; set; }
    }
}
