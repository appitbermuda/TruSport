using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class BasketballRosterListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomePlayerID { get; set; }
        public string HomePlayerName { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayPlayerID { get; set; }
        public string AwayPlayerName { get; set; }
        public int? HomeJerseyNumber { get; set; }
        public int? AwayJerseyNumber { get; set; }
        public bool IsStarter { get; set; }
        public bool IsHomeTeam { get; set; }
    }
}
