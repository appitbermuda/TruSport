using System;
using System.Collections.Generic;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Models.Imports
{
    public class ImportCricketFixtures
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<CricketFixtures> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportBowlingFixtures
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<BowlingFixtures> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportFootballFixtures
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<Fixtures> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportTransfers
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<Transfers> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportRunStats
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<RunStats> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportWicketStats
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<WicketStats> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportBowlingTeam
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<BowlingTeams> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportBowlingGames
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<BowlingGames> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportBowlingLeagueStandings
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<BowlingLeagueStandings> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportBowlingPlayers
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<BowlingPlayerSeasons> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }

    public class ImportTicketMembers
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<TicketMembers> ErrorRows { get; set; }
        public byte[] ErrorFile { get; set; }
    }
}
