using System;
namespace OnTrackWebService.Models
{
    public class TicketReport
    {
        public string FixtureID { get; set; }
        public string Fixture { get; set; }
        public SportEvent SportEvent { get; set; }
        public string Date { get; set; }
    }
}
