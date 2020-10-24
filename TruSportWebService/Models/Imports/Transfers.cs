using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class Transfers
    {
        public string Name { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeam { get; set; }
        public int Season { get; set; }
        public string League { get; set; }
        public string IsLateTransfer { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
