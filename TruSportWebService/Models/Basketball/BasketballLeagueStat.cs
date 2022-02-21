using System;
namespace OnTrackWebService.Models.Basketball
{
    public class BasketballLeagueStat
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
        public int Points { get; set; }
        public int ThreePoints { get; set; }
        public int Rebounds { get; set; }
        public int Stat { get; set; }
    }
}
