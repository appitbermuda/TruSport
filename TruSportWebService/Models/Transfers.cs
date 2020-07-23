using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class vTransfers
    {
        public string PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeam { get; set; }
        public string Date { get; set; }
    }

    public class Transfer
    {
        public string ID { get; set; }
        public string PlayerName { get; set; }
        public string PreviousTeam { get; set; }
        public string NewTeamID { get; set; }
        public string SeasonID { get; set; }
        public string SportID { get; set; }
        public string Date { get; set; }
        public bool IsLateTransfer { get; set; }

        [ForeignKey("NewTeamID")]
        public Team NewTeam { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }


        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
