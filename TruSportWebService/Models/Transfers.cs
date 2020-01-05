using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Transfers
    {
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeam { get; set; }
        public string Date { get; set; }
    }
}
