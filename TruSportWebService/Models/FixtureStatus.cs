using System;
namespace OnTrackWebService.Models
{
    public class FixtureStatus
    {
        public string FixtureID { get; set; }
        public bool IsPostponed { get; set; }
        public bool IsCancelled { get; set; }
    }
}
