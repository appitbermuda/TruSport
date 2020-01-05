using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace TruSport.Model
{
    public class Flyer
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class Transfers
    {
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeam { get; set; }
        public string Date { get; set; }
    }

    public class Season
    {
        public string ID { get; set; }
        public int Key { get; set; }
        public string Date { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class Teams
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int LeagueID { get; set; }
        public int HomeFieldID { get; set; }
        public string TeamLogo { get; set; }
        public string Alias { get; set; }
    }

    public class TeamSeason
    {
        public string ID { get; set; }
        public string LeagueID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }

        //[ForeignKey("LeagueID")]
        [Ignore]
        public League League { get; set; }

        //[ForeignKey("SeasonID")]
        [Ignore]
        public Season Season { get; set; }


        //[ForeignKey("TeamID")]
        [Ignore]
        public Team Team { get; set; }

    }

    //public class Team
    //{
    //    string id;
    //    string name;
    //    string teamLogo;
    //    string homeFieldID;
    //    string leagueID;
    //    string alias;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "name")]
    //    public string Name
    //    {
    //        get { return name; }
    //        set { name = value; }
    //    }

    //    [JsonProperty(PropertyName = "teamLogo")]
    //    public string TeamLogo
    //    {
    //        get { return teamLogo; }
    //        set { teamLogo = value; }
    //    }

    //    [JsonProperty(PropertyName = "homeFieldID")]
    //    public string HomeFieldID
    //    {
    //        get { return homeFieldID; }
    //        set { homeFieldID = value; }
    //    }

    //    [JsonProperty(PropertyName = "alias")]
    //    public string Alias
    //    {
    //        get { return alias; }
    //        set { alias = value; }
    //    }

    //    [JsonProperty(PropertyName = "leagueID")]
    //    public string LeagueID
    //    {
    //        get { return leagueID; }
    //        set { leagueID = value; }
    //    }

    //    [Version]
    //    public string Version { get; set; }
    //}

    public class Team
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string LeagueID { get; set; }
        public string HomeFieldID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }


        //[ForeignKey("LeagueID")]
        [Ignore]
        public League League { get; set; }

        //[ForeignKey("HomeFieldID")]
        [Ignore]
        public Field Field { get; set; }

        //[Ignore]
        //public virtual List<Player> Players { get; set; }

        [Ignore]
        public virtual List<Coach> Coaches { get; set; }
    }

    public class TeamProfile
    {
        public int ID { get; set; }
        public int CoachID { get; set; }
        public string Coach { get; set; }
        public int AssitantCoachID { get; set; }
        public string AssitantCoach { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
    }

    public class LeagueTables
    {
        public int ID { get; set; }
        public int TeamID { get; set; }
        public int Position { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Draw { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int GamesPlayed { get; set; }
        public int LeagueID { get; set; }
    }

    //public class LeagueTable
    //{
    //    string id;
    //    string teamID;
    //    int position;
    //    int win;
    //    int loss;
    //    int draw;
    //    int points;
    //    int goalsFor;
    //    int goalsAgainst;
    //    int goalDifference;
    //    string leagueID;
    //    int gamesPlayed;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "teamID")]
    //    public string TeamID
    //    {
    //        get { return teamID; }
    //        set { teamID = value; }
    //    }

    //    [JsonProperty(PropertyName = "position")]
    //    public int Position
    //    {
    //        get { return position; }
    //        set { position = value; }
    //    }

    //    [JsonProperty(PropertyName = "win")]
    //    public int Win
    //    {
    //        get { return win; }
    //        set { win = value; }
    //    }

    //    [JsonProperty(PropertyName = "loss")]
    //    public int Loss
    //    {
    //        get { return loss; }
    //        set { loss = value; }
    //    }

    //    [JsonProperty(PropertyName = "draw")]
    //    public int Draw
    //    {
    //        get { return draw; }
    //        set { draw = value; }
    //    }

    //    [JsonProperty(PropertyName = "points")]
    //    public int Points
    //    {
    //        get { return points; }
    //        set { points = value; }
    //    }

    //    [JsonProperty(PropertyName = "goalsFor")]
    //    public int GoalsFor
    //    {
    //        get { return goalsFor; }
    //        set { goalsFor = value; }
    //    }

    //    [JsonProperty(PropertyName = "goalsAgainst")]
    //    public int GoalsAgainst
    //    {
    //        get { return goalsAgainst; }
    //        set { goalsAgainst = value; }
    //    }

    //    [JsonProperty(PropertyName = "goalDifference")]
    //    public int GoalDifference
    //    {
    //        get { return goalDifference; }
    //        set { goalDifference = value; }
    //    }

    //    [JsonProperty(PropertyName = "leagueID")]
    //    public string LeagueID
    //    {
    //        get { return leagueID; }
    //        set { leagueID = value; }
    //    }

    //    [JsonProperty(PropertyName = "gamesPlayed")]
    //    public int GamesPlayed
    //    {
    //        get { return gamesPlayed; }
    //        set { gamesPlayed = value; }
    //    }
    //}

    public class LeagueTable
    {
        public string ID { get; set; }
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
        public bool IsSelectedTeam { get; set; }

        //[ForeignKey("TeamID")]
        public Team Team { get; set; }

        //[ForeignKey("LeagueID")]
        public League League { get; set; }

        public Season Season { get; set; }
    }

    public class Fixtures
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int HomeTeamID { get; set; }
        public int AwayTeamID { get; set; }
        public int FieldID { get; set; }
        public int LeagueID { get; set; }
        public int MatchTypeID { get; set; }
    }

    //public class Fixture
    //{
    //    string id;
    //    DateTime date;
    //    DateTime time;
    //    string homeTeamID;
    //    string awayTeamID;
    //    string fieldID;
    //    string leagueID;
    //    string matchTypeID;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "fieldID")]
    //    public string FieldID
    //    {
    //        get { return fieldID; }
    //        set { fieldID = value; }
    //    }

    //    [JsonProperty(PropertyName = "leagueID")]
    //    public string LeagueID
    //    {
    //        get { return leagueID; }
    //        set { leagueID = value; }
    //    }

    //    [JsonProperty(PropertyName = "matchTypeID")]
    //    public string MatchTypeID
    //    {
    //        get { return matchTypeID; }
    //        set { matchTypeID = value; }
    //    }

    //    [JsonProperty(PropertyName = "homeTeamID")]
    //    public string HomeTeamID
    //    {
    //        get { return homeTeamID; }
    //        set { homeTeamID = value; }
    //    }

    //    [JsonProperty(PropertyName = "awayTeamID")]
    //    public string AwayTeamID
    //    {
    //        get { return awayTeamID; }
    //        set { awayTeamID = value; }
    //    }

    //    [JsonProperty(PropertyName = "date")]
    //    public DateTime Date
    //    {
    //        get { return date; }
    //        set { date = value; }
    //    }

    //    [JsonProperty(PropertyName = "time")]
    //    public DateTime Time
    //    {
    //        get { return time; }
    //        set { time = value; }
    //    }

    //    [Version]
    //    public string Version { get; set; }
    //}

    public class Fixture
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string GameTime { get; set; }
        public string SelectedTeamID { get; set; }
        public string SelectedTeamResult { get; set; }
        public bool IsPostponed { get; set; }
        //public DateTime FixtureTime { get; set; }

        [Ignore]
        //public DateTime FixtureTime => Date.Add(new DateTime().AddDays(5).Add(TimeSpan.Parse(Time)).ToLocalTime().TimeOfDay);
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? Date.AddDays(1).Add(TimeSpan.Parse(Time)).ToLocalTime() : Date.Add(TimeSpan.Parse(Time)).ToLocalTime();

        //[ForeignKey("HomeTeamID")]
        [Ignore]
        public Team HomeTeam { get; set; }

        //[ForeignKey("AwayTeamID")]
        [Ignore]
        public Team AwayTeam { get; set; }

        //[ForeignKey("FieldID")]
        [Ignore]
        public Field Field { get; set; }

        //[ForeignKey("LeagueID")]
        [Ignore]
        public League League { get; set; }

        //[ForeignKey("MatchTypeID")]
        [Ignore]
        public MatchType MatchType { get; set; }

        [Ignore]
        public Match Match { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<MatchRoster> MatchRosters { get; set; }
    }

    public class MatchTypes
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class MatchType
    {
        string id;
        string name;
        bool isTable;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty(PropertyName = "isTable")]
        public bool IsTable
        {
            get { return isTable; }
            set { isTable = value; }
        }

        [Version]
        public string Version { get; set; }
    }

    public class Matchs
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int FixtureID { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public int HomeYellowCards { get; set; }
        public int AwayYellowCards { get; set; }
        public int HomeRedCards { get; set; }
        public int AwayRedCards { get; set; }

    }

    public class Match
    {
        //string id;
        //string fixtureID;
        //int homeTeamScore;
        //int awayTeamScore;
        //int homeYellowCards;
        //int awayYellowCards;
        //int homeRedCards;
        //int awayRedCards;

        //[PrimaryKey]
        //[JsonProperty(PropertyName = "id")]
        //public string ID
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        //[JsonProperty(PropertyName = "fixtureID")]
        //public string FixtureID
        //{
        //    get { return fixtureID; }
        //    set { fixtureID = value; }
        //}

        //[JsonProperty(PropertyName = "homeTeamScore")]
        //public int HomeTeamScore
        //{
        //    get { return homeTeamScore; }
        //    set { homeTeamScore = value; }
        //}

        //[JsonProperty(PropertyName = "awayTeamScore")]
        //public int AwayTeamScore
        //{
        //    get { return awayTeamScore; }
        //    set { awayTeamScore = value; }
        //}

        //[JsonProperty(PropertyName = "homeYellowCards")]
        //public int HomeYellowCards
        //{
        //    get { return homeYellowCards; }
        //    set { homeYellowCards = value; }
        //}

        //[JsonProperty(PropertyName = "awayYellowCards")]
        //public int AwayYellowCards
        //{
        //    get { return awayYellowCards; }
        //    set { awayYellowCards = value; }
        //}

        //[JsonProperty(PropertyName = "homeRedCards")]
        //public int HomeRedCards
        //{
        //    get { return homeRedCards; }
        //    set { homeRedCards = value; }
        //}

        //[JsonProperty(PropertyName = "awayRedCards")]
        //public int AwayRedCards
        //{
        //    get { return awayRedCards; }
        //    set { awayRedCards = value; }
        //}

        //[Version]
        //public string Version { get; set; }

            [PrimaryKey]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? HomeYellowCards { get; set; }
        public int? AwayYellowCards { get; set; }
        public int? HomeRedCards { get; set; }
        public int? AwayRedCards { get; set; }
        public bool? IsPenalties { get; set; }
        public int? HomeTeamPenalty { get; set; }
        public int? AwayTeamPenalty { get; set; }
        //[ForeignKey("FixtureID")]

        [Ignore]
        public Fixture Fixture { get; set; }
    }

    public class MatchRosters
    {
        public int ID { get; set; }
        public int TeamID { get; set; }
        public int MatchID { get; set; }
        public int PlayerID { get; set; }
        public int JerseyNumber { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public bool IsStarter { get; set; }
        public bool IsSub { get; set; }
    }

    public class MatchRoster
    {
        
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string SubstitutePlayerID { get; set; }
        public int? JerseyNumber { get; set; }
        public bool IsStarter { get; set; }
        public int? SubstituteTime { get; set; }

        //[ForeignKey("FixtureID")]
        [Ignore]
        public Fixture Fixture { get; set; }

        //[ForeignKey("TeamID")]
        [Ignore]
        public Team Team { get; set; }

        //[ForeignKey("PlayerID")]
        [Ignore]
        public Player Player { get; set; }

        //[ForeignKey("SubstitutePlayerID")]
        [Ignore]
        public Player SubstitutePlayer { get; set; }

        [Ignore]
        public virtual List<MatchStat> MatchStats { get; set; }
    }

    public class RosterListView
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

    public class MatchStat
    {

        public string ID { get; set; }
        public string MatchRosterID { get; set; }
        public string AssistPlayerID { get; set; }
        public int? Goal { get; set; }
        public int? YellowCard { get; set; }
        public int? RedCard { get; set; }
        public int? GoalTime { get; set; }
        public int? YellowCardTime { get; set; }
        public int? RedCardTime { get; set; }

        //[ForeignKey("SubstitutePlayerID")]
        [Ignore]
        public Player AssistPlayer { get; set; }

    }

    public class MatchRosterSummary
    {

        public string ID { get; set; }
        public string MatchStatID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string SubstitutePlayerID { get; set; }
        public string SubstitutePlayerName { get; set; }
        public string AssistPlayerID { get; set; }
        public string AssistPlayerName { get; set; }
        //public int? JerseyNumber { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public bool IsSub { get; set; }
        public bool IsHomeTeam { get; set; }
        public int Minute { get; set; }

        ////[ForeignKey("FixtureID")]
        //[Ignore]
        //public Fixture Fixture { get; set; }

        ////[ForeignKey("TeamID")]
        //[Ignore]
        //public Team Team { get; set; }

        ////[ForeignKey("PlayerID")]
        //[Ignore]
        //public Player Player { get; set; }

        ////[ForeignKey("SubstitutePlayerID")]
        //[Ignore]
        //public Player SubsitutePlayer { get; set; }

        ////[ForeignKey("AssistPlayerID")]
        //[Ignore]
        //public Player AssistPlayer { get; set; }
    }

    public class MatchPlayer
    {
        public string MatchPlayerID { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }

    public class SeasonPlayer
    {
        public int SeasonPlayerID { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int MatchesPlayed { get; set; }
    }

    public class Players
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TeamID { get; set; }
        public int Number { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Played { get; set; }

    }

    public class Player
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JerseyNumber { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Played { get; set; }

        [Ignore]
        public string Name => FirstName + " " + LastName;

        [Ignore]
        public bool IsStarter { get; set; }

    }

    public class PlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public int? Goals { get; set; }
        public int? YellowCards { get; set; }
        public int? RedCards { get; set; }
        public bool IsActive { get; set; }

        //[ForeignKey("PlayerID")]
        [Ignore]
        public Player Player { get; set; }

        //[ForeignKey("SeasonID")]
        [Ignore]
        public Season Season { get; set; }

        //[ForeignKey("TeamID")]
        [Ignore]
        public Team Team { get; set; }

    }

    public class UserTeam
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string TeamID { get; set; }

        [Ignore]
        public User User { get; set; }

        [Ignore]
        public Team Team { get; set; }

    }

    //public class Player
    //{
    //    string id;
    //    string firstName;
    //    string lastName;
    //    string teamID;
    //    int? number;
    //    int goals;
    //    int yellowCards;
    //    int redCards;
    //    int played;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "firstName")]
    //    public string FirstName
    //    {
    //        get { return firstName; }
    //        set { firstName = value; }
    //    }

    //    [JsonProperty(PropertyName = "lastName")]
    //    public string LastName
    //    {
    //        get { return lastName; }
    //        set { lastName = value; }
    //    }

    //    [JsonProperty(PropertyName = "teamID")]
    //    public string TeamID
    //    {
    //        get { return teamID; }
    //        set { teamID = value; }
    //    }

    //    [JsonProperty(PropertyName = "JerseyNumber")]
    //    public int? JerseyNumber
    //    {
    //        get { return number; }
    //        set { number = value; }
    //    }

    //    [JsonProperty(PropertyName = "goals")]
    //    public int Goals
    //    {
    //        get { return goals; }
    //        set { goals = value; }
    //    }

    //    [JsonProperty(PropertyName = "yellowCards")]
    //    public int YellowCards
    //    {
    //        get { return yellowCards; }
    //        set { yellowCards = value; }
    //    }

    //    [JsonProperty(PropertyName = "redCards")]
    //    public int RedCards
    //    {
    //        get { return redCards; }
    //        set { redCards = value; }
    //    }

    //    [JsonProperty(PropertyName = "played")]
    //    public int Played
    //    {
    //        get { return played; }
    //        set { played = value; }
    //    }

    //    [Ignore]
    //    public string Name => FirstName + " " + LastName;

    //    [Ignore]
    //    public bool IsStarter { get; set; }

    //    [Version]
    //    public string Version { get; set; }
    //}

    public class Coaches
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TeamID { get; set; }
    }

    public class Coach
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamID { get; set; }


        [Ignore]
        public string Name => FirstName + " " + LastName;

        [Ignore]
        public Team Team { get; set; }
    }

    public class RosterCoachListView
    {
        [PrimaryKey]
        public string HomeCoachID { get; set; }
        public string HomeCoachName { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayCoachID { get; set; }
        public string AwayCoachName { get; set; }
        public string AwayTeamID { get; set; }
    }

    public class Fields
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Field
    {
        string id;
        string name;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Version]
        public string Version { get; set; }
    }

    public class Leagues
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class League
    {
        string id;
        string name;

        [PrimaryKey]
        [JsonProperty(PropertyName = "id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Version]
        public string Version { get; set; }
    }

    public class Favourites
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string UserID { get; set; }
    }

    //public class Favourite
    //{
    //    string id;
    //    string type;
    //    string favValue;
    //    string logo;
    //    string userID;

    //    [PrimaryKey]
    //    [JsonProperty(PropertyName = "id")]
    //    public string ID
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    [JsonProperty(PropertyName = "type")]
    //    public string Type
    //    {
    //        get { return type; }
    //        set { type = value; }
    //    }

    //    [JsonProperty(PropertyName = "value")]
    //    public string Value
    //    {
    //        get { return favValue; }
    //        set { favValue = value; }
    //    }

    //    [JsonProperty(PropertyName = "logo")]
    //    public string Logo
    //    {
    //        get { return logo; }
    //        set { logo = value; }
    //    }

    //    [JsonProperty(PropertyName = "userID")]
    //    public string UserID
    //    {
    //        get { return userID; }
    //        set { userID = value; }
    //    }

    //    [Version]
    //    public string Version { get; set; }
    //}

    public class Favourite
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Type { get; set; }
        public string TeamID { get; set; }
        public string FixtureID { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }
    }

    public class News
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
    }
}
