using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models.Imports
{
    public class ImportCricketFixtures
    {
        public string Message { get; set; }
        public string Exception { get; set; }
        public List<CricketFixtures> ErrorRows { get; set; }
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
}
