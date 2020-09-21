using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Fielding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string CricketRosterID { get; set; }
        public int Over { get; set; }
        public int Maiden { get; set; }
        public int Run { get; set; }
        public int Wicket { get; set; }
        public int NoBall { get; set; }
        public int Wide { get; set; }


        [ForeignKey("CricketRosterID")]
        public CricketRoster CricketRoster { get; set; }
    }
}
