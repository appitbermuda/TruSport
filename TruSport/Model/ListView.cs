using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace TruSport.Model
{
    public class TeamListView
    {
        public string ID { get; set; }
        public string LeagueID { get; set; }
        public string HomeFieldID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string LeagueName { get; set; }
        public string HomeFieldName { get; set; }
        public string TeamLogo { get; set; }
    }

    public class LeagueTableListView
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string LeagueID { get; set; }
        public string TeamName { get; set; }
        public int Position { get; set; }
        public int GamesPlayed { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public string LeagueName { get; set; }
        public bool IsSelectedTeam { get; set; }
        public bool IsTable { get; set; }
        public string MatchTypeName { get; set; }
    }

    public class FixtureListView
    {
        public string ID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string MatchTypeID { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string FieldName { get; set; }
        public string LeagueName { get; set; }
        public string MatchTypeName { get; set; }
    }

    public class MatchListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string LeagueID { get; set; }
        public string FieldID { get; set; }
        public string MatchTypeID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public int HomeYellowCards { get; set; }
        public int AwayYellowCards { get; set; }
        public int HomeRedCards { get; set; }
        public int AwayRedCards { get; set; }
        public DateTime MatchDate { get; set; }
        public DateTime MatchTime { get; set; }
        public string LeagueName { get; set; }
        public string FieldName { get; set; }
        public string MatchTypeName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string MatchDateTime { get { return MatchDate.Add(MatchTime.TimeOfDay).ToString("f"); } }
    }

    public class MatchRosterListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
        public int JerseyNumber { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public bool IsStarter { get; set; }
        public bool IsSub { get; set; }

        public MatchRosterListView Clone()
        {
            return new MatchRosterListView()
            {
                ID = ID,
                FixtureID = FixtureID,
                TeamID = TeamID,
                PlayerID = PlayerID,
                TeamName = TeamName,
                PlayerName = PlayerName,
                JerseyNumber = JerseyNumber,
                Goals = Goals,
                YellowCards = YellowCards,
                RedCards = RedCards,
                IsStarter = IsStarter,
                IsSub = IsSub
            };

        }
    }

    public class PlayerListView
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public int JerseyNumber { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Played { get; set; }
        public string PlayerName { get { return FirstName + " " + LastName; } }
    }

    public class CoachListView
    {
        public string ID { get; set; }
        public string TeamID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
    }
}
