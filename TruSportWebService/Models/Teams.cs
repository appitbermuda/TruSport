using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class Teams
    {
        public string Name { get; set; }
        public string HomeField { get; set; }
        public string League { get; set; }
        public string Sport { get; set; }
        public string Alias { get; set; }
        public string TeamLogo { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
