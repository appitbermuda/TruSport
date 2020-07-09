using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
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


        [ForeignKey("CricketRosterID")]
        public CricketRoster CricketRoster { get; set; }

        //[ForeignKey("OutTypeID")]
        //public OutType OutType { get; set; }

        [ForeignKey("FirstFielderID")]
        public Fielding FirstFielder { get; set; }

        [ForeignKey("SecondFielderID")]
        public Fielding SecondFielder { get; set; }
    }
}
