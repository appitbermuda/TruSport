using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class LTable
    {
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDiff { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class LeagueTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDiff { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class CricketLeagueTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public decimal NetRunRate { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class CricketFirstDivisionTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class CricketPremierLeagueTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class PremierLeagueTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDiff { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class FirstDivisionTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDiff { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }

    public class CoronaLeagueTable
    {
        public string TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Draws { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDiff { get; set; }

        [NotMapped]
        public bool IsSelectedTeam { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }
    }
    //public class LeagueTable
    //{
    //    public string ID { get; set; }
    //    public string TeamID { get; set; }
    //    public string LeagueID { get; set; }
    //    public int Position { get; set; }
    //    public int Win { get; set; }
    //    public int Loss { get; set; }
    //    public int Draw { get; set; }
    //    public int Points { get; set; }
    //    public int GoalsFor { get; set; }
    //    public int GoalsAgainst { get; set; }
    //    public int GoalDifference { get; set; }
    //    public int GamesPlayed { get; set; }

    //    [ForeignKey("TeamID")]
    //    public Team Team { get; set; }

    //    [ForeignKey("LeagueID")]
    //    public League League { get; set; }
    //}

}
