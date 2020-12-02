using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace TruSport.Model
{
    public class Batting
    {
        public string ID { get; set; }
        public string CricketRosterID { get; set; }
        public string OutTypeID { get; set; }
        public string FirstFielderID { get; set; }
        public string SecondFielderID { get; set; }
        public int Order { get; set; }
        public int? Run { get; set; }
        public int? Ball { get; set; }
        public int? Four { get; set; }
        public int? Six { get; set; }


        [Ignore]
        public CricketRoster CricketRoster { get; set; }

        //[ForeignKey("OutTypeID")]
        //public OutType OutType { get; set; }

        [Ignore]
        public Fielding FirstFielder { get; set; }

        [Ignore]
        public Fielding SecondFielder { get; set; }
    }

    public class Fielding
    {
        public string ID { get; set; }
        public string CricketRosterID { get; set; }
        public int Over { get; set; }
        public int Maiden { get; set; }
        public int Run { get; set; }
        public int Wicket { get; set; }
        public int NoBall { get; set; }
        public int Wide { get; set; }


        [Ignore]
        public CricketRoster CricketRoster { get; set; }
    }

    public class CricketMatch
    {
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public decimal? Overs { get; set; }
        public int? Extras { get; set; }
        public int? Bye { get; set; }
        public int? LegBye { get; set; }
        public int? NoBall { get; set; }
        public int? Wide { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team Team { get; set; }
    }

    public class CricketFieldingRoster
    {
        public string ID { get; set; }
        public string CricketScoreID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public int Overs { get; set; }
        public int Maidens { get; set; }
        public int Runs { get; set; }
        public int Wickets { get; set; }
        public int NoBalls { get; set; }
        public int Wides { get; set; }


        [Ignore]
        public CricketScore CricketScore { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }
    }

    public class CricketBattingRoster
    {
        public string ID { get; set; }
        public string CricketScoreID { get; set; }
        public string OutTypeID { get; set; }
        public string FirstFielderRosterID { get; set; }
        public string SecondFielderRosterID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public int Order { get; set; }
        public int? Runs { get; set; }
        public int? Balls { get; set; }
        public int? Four { get; set; }
        public int? Six { get; set; }


        [Ignore]
        public CricketScore CricketScore { get; set; }

        [Ignore]
        public OutType OutType { get; set; }

        [Ignore]
        public CricketFieldingRoster FirstFielderRoster { get; set; }

        [Ignore]
        public CricketFieldingRoster SecondFielderRoster { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }
    }

    public class OutType
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
    }

    public class CricketScore
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string BattingTeamID { get; set; }
        public string BowlingTeamID { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public decimal? Overs { get; set; }
        public int? Extras { get; set; }
        public int? Bye { get; set; }
        public int? LegBye { get; set; }
        public int? NoBall { get; set; }
        public int? Wide { get; set; }
        public int Innings { get; set; }
        public int MatchInnings { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team BattingTeam { get; set; }

        [Ignore]
        public Team BowlingTeam { get; set; }
    }

    public class AwardType
    {
        public string ID { get; set; }
        public string Name { get; set; }

    }

    public class Award
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string AwardTypeID { get; set; }
        public string Image { get; set; }
        public string SeasonID { get; set; }
        public string SportID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual Player Player { get; set; }
        public virtual AwardType AwardType { get; set; }
        public virtual Season Season { get; set; }
        public virtual Sport Sport { get; set; }
    }

    public class Flyer
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class DefaultSport
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string SportID { get; set; }
        public string Sport { get; set; }
    }

    public class Sport
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class Transfers
    {
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeam { get; set; }
        public string Date { get; set; }
    }

    public class Transfer
    {
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeamID { get; set; }
        public string SeasonID { get; set; }
        public string SportID { get; set; }
        public string Date { get; set; }
        public bool IsLateTransfer { get; set; }

        [Ignore]
        public Team NewTeam { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public Sport Sport { get; set; }
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

        [Ignore]
        public virtual List<Transfer> Transfers { get; set; }

        [Ignore]
        public virtual List<CricketFixture> CricketFixtures { get; set; }

        [Ignore]
        public virtual List<CricketFixture> CricketForm { get; set; }

        [Ignore]
        public virtual List<Fixture> Fixtures { get; set; }

        [Ignore]
        public virtual List<Fixture> Form { get; set; }

        [Ignore]
        public virtual List<PlayerSeason> Players { get; set; }

        [Ignore]
        public virtual List<LeagueTable> FootballTable { get; set; }

        [Ignore]
        public virtual List<CricketLeagueTable> CricketTable { get; set; }

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

        [Ignore]
        public Field Field { get; set; }

        [Ignore]
        public virtual List<Coach> Coaches { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public virtual LeagueTable FootballTable { get; set; }

        [Ignore]
        public virtual CricketLeagueTable CricketTable { get; set; }

        [Ignore]
        public virtual BowlingLeagueStanding BowlingLeagueStanding { get; set; }

        [Ignore]
        public virtual List<TeamSeason> TeamSeasons { get; set; }

        [Ignore]
        public virtual List<CricketFixture> CricketFixtures { get; set; }

        [Ignore]
        public virtual List<Fixture> Fixtures { get; set; }

        [Ignore]
        public virtual List<BowlingFixture> BowlingFixtures { get; set; }

        [Ignore]
        public virtual List<CricketFixture> CricketForm { get; set; }

        [Ignore]
        public virtual List<BowlingFixture> BowlingForm { get; set; }

        [Ignore]
        public virtual List<Fixture> Form { get; set; }

        [Ignore]
        public virtual List<PlayerSeason> Players { get; set; }

        [Ignore]
        public virtual List<BowlingPlayerSeason> BowlingPlayers { get; set; }

        [Ignore]
        public virtual bool IsFavourite { get; set; }
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
        public bool IsSelectedTeam { get; set; }

        //[ForeignKey("TeamID")]
        public Team Team { get; set; }

        //[ForeignKey("LeagueID")]
        public League League { get; set; }

        public Season Season { get; set; }
    }

    public class CricketLeagueTable
    {
        public string ID { get; set; }
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
        public bool IsCancelled { get; set; }
        //public DateTime FixtureTime { get; set; }

        [Ignore]
        //public DateTime FixtureTime => Date.Add(new DateTime().AddDays(5).Add(TimeSpan.Parse(Time)).ToLocalTime().TimeOfDay);
        public DateTime FixturesTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);
        //public DateTime FixtureTime => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [Ignore]
        public DateTime FixtureTimeTest => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [Ignore]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [Ignore]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [Ignore]
        public bool IsPostponedOrCancelled { get; set; }

        [Ignore]
        public int TicketAvailable { get; set; }

        [Ignore]
        public Team HomeTeam { get; set; }

        [Ignore]
        public Team AwayTeam { get; set; }

        [Ignore]
        public Field Field { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public MatchType MatchType { get; set; }

        [Ignore]
        public Match Match { get; set; }

        [Ignore]
        public virtual List<LeagueTable> LeagueTable { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<Fixture> HeadToHead { get; set; }

        [Ignore]
        public ObservableCollection<MatchRoster> MatchRosters { get; set; }

        [Ignore]
        public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }

    public class CricketFixture
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
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        [Ignore]
        public string SelectedTeamID { get; set; }

        [Ignore]
        public string SelectedTeamResult { get; set; }

        [Ignore]
        public DateTime FixtureTime => TimeSpan.Parse(Time) < TimeSpan.Parse("04:01") ? TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.AddDays(1).Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local) : TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) ?
                        TimeZoneInfo.ConvertTime(Date.Add((TimeSpan.Parse(Time) + (TimeZoneInfo.Local.GetUtcOffset(new DateTime().Date + TimeSpan.Parse(Time)) - TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)))), TimeZoneInfo.Utc, TimeZoneInfo.Local) :
                        TimeZoneInfo.ConvertTime(Date.Add(TimeSpan.Parse(Time)), TimeZoneInfo.Utc, TimeZoneInfo.Local);

        [Ignore]
        public string PostOrCanc => IsPostponed ? "Post." : IsCancelled ? "Canc." : String.Empty;

        [Ignore]
        public bool IsPostponedOrCancelled { get; set; }

        [Ignore]
        public Team HomeTeam { get; set; }

        [Ignore]
        public Team AwayTeam { get; set; }

        [Ignore]
        public Field Field { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public MatchType MatchType { get; set; }

        [Ignore]
        public CricketMatch CricketMatch { get; set; }

        [Ignore]
        public virtual List<CricketLeagueTable> LeagueTable { get; set; }

        [Ignore]
        public Season Season { get; set; }

        [Ignore]
        public ObservableCollection<CricketFixture> HeadToHead { get; set; }

        [Ignore]
        public ObservableCollection<CricketRoster> CricketRosters { get; set; }

        [Ignore]
        public ObservableCollection<MatchInning> MatchInnings { get; set; }

        [Ignore]
        public string HomeTeamScore { get; set; }

        [Ignore]
        public string AwayTeamScore { get; set; }

        [Ignore]
        public string MatchResult { get; set; }

        //[Ignore]
        //public virtual List<MatchRosterSummary> MatchRosterSummary { get; set; }
    }

    public class MatchInning
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string BattingTeamID { get; set; }
        public string FieldingTeamID { get; set; }
        public int? Run { get; set; }
        public int? Wicket { get; set; }
        public decimal? Over { get; set; }
        public int? Order { get; set; }
        public int Inning { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team BattingTeam { get; set; }

        [Ignore]
        public Team FieldingTeam { get; set; }
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

        [Ignore]
        public bool IsHomeTeam { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public Player SubstitutePlayer { get; set; }

        [Ignore]
        public virtual List<MatchStat> MatchStats { get; set; }
    }

    public class CricketRoster
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public int? JerseyNumber { get; set; }
        public bool IsReserve { get; set; }
        public bool IsColt { get; set; }

        [Ignore]
        public bool IsHomeTeam { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public ObservableCollection<Batting> Batters { get; set; }

        [Ignore]
        public ObservableCollection<Fielding> Fielders { get; set; }
    }

    public class BowlingRosterListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomePlayerName { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayPlayerName { get; set; }
        public int Position { get; set; }
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

    //public class MatchRosterSummary
    //{

    //    public string ID { get; set; }
    //    public string MatchStatID { get; set; }
    //    public string FixtureID { get; set; }
    //    public string TeamID { get; set; }
    //    public string PlayerID { get; set; }
    //    public string PlayerName { get; set; }
    //    public string SubstitutePlayerID { get; set; }
    //    public string SubstitutePlayerName { get; set; }
    //    public string AssistPlayerID { get; set; }
    //    public string AssistPlayerName { get; set; }
    //    //public int? JerseyNumber { get; set; }
    //    public int Goal { get; set; }
    //    public int Assist { get; set; }
    //    public int YellowCard { get; set; }
    //    public int RedCard { get; set; }
    //    public bool IsSub { get; set; }
    //    public bool IsHomeTeam { get; set; }
    //    public int Minute { get; set; }

    //    ////[ForeignKey("FixtureID")]
    //    //[Ignore]
    //    //public Fixture Fixture { get; set; }

    //    ////[ForeignKey("TeamID")]
    //    //[Ignore]
    //    //public Team Team { get; set; }

    //    ////[ForeignKey("PlayerID")]
    //    //[Ignore]
    //    //public Player Player { get; set; }

    //    ////[ForeignKey("SubstitutePlayerID")]
    //    //[Ignore]
    //    //public Player SubsitutePlayer { get; set; }

    //    ////[ForeignKey("AssistPlayerID")]
    //    //[Ignore]
    //    //public Player AssistPlayer { get; set; }
    //}

    public class MatchRosterSummary
    {

        public string ID { get; set; }
        public string MatchRosterID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string SubstitutePlayerID { get; set; }
        public string AssistPlayerID { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public bool IsSub { get; set; }
        public bool IsHomeTeam { get; set; }
        public int Minute { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public Player Player { get; set; }

        [Ignore]
        public Player SubsitutePlayer { get; set; }

        [Ignore]
        public Player AssistPlayer { get; set; }
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

    public class CricketPlayerSeason
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string TeamID { get; set; }
        public string SeasonID { get; set; }
        public int? GamesPlayed { get; set; }
        public bool IsActive { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public int? BallsFaced { get; set; }
        public int? RunsConceded { get; set; }

        //[ForeignKey("PlayerID")]
        public Player Player { get; set; }

        //[ForeignKey("SeasonID")]
        public Season Season { get; set; }

        //[ForeignKey("TeamID")]
        public Team Team { get; set; }

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
        public string ID { get; set; }
        public string Name { get; set; }
        public string SportID { get; set; }
        public int Order { get; set; }

        [Ignore]
        public Sport Sport { get; set; }
    }

    public class Favourites
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string UserID { get; set; }
    }

    public class Favourite
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Type { get; set; }
        public string TeamID { get; set; }
        public string FixtureID { get; set; }
        public string CricketFixtureID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string Sport { get; set; }

        [Ignore]
        public League League { get; set; }

        [Ignore]
        public Team Team { get; set; }

        [Ignore]
        public BowlingFixture BowlingFixture { get; set; }

        [Ignore]
        public CricketFixture CricketFixture { get; set; }

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
