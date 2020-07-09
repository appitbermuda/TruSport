using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
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
        public int Stat { get; set; }
    }

    public class GoalsConcededByTeam
    {
        public string TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public int Goals { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
    }

    public class GoalsScoredByPlayer
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

    public class RunsByPlayer
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
        public int Stat { get; set; }
    }

    public class WicketsByPlayer
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
        public int Stat { get; set; }
    }

    public class GoalsScoredByTeam
    {
        public string TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public int Goals { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
    }
}
