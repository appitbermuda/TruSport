using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class spLiveFixtures
    {
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamAlias { get; set; }
        public string HomeTeamLogo { get; set; }
        public int? HomeTeamScore { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamAlias { get; set; }
        public string AwayTeamLogo { get; set; }
        public int? AwayTeamScore { get; set; }
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string MatchTypeID { get; set; }
        public string MatchTypeName { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }

    public class spLiveCricketFixtures
    {
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamAlias { get; set; }
        public string HomeTeamLogo { get; set; }
        public string HomeTeamScore { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamAlias { get; set; }
        public string AwayTeamLogo { get; set; }
        public string AwayTeamScore { get; set; }
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string MatchTypeID { get; set; }
        public string MatchTypeName { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}
