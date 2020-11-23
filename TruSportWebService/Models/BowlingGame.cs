using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingGame
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingRosterID { get; set; }
        public int Score { get; set; }
        public int Game { get; set; }
        public int? Points { get; set; }

        [ForeignKey("BowlingRosterID")]
        public BowlingRoster BowlingRoster { get; set; }

        [NotMapped]
        public bool Win { get; set; }
    }

    public class BowlingGameResult
    {
        public string ID { get; set; }
        public string BowlingRosterID1 { get; set; }
        public string BowlingRosterID2 { get; set; }
        public int? Score1 { get; set; }
        public int? Score2 { get; set; }
        public int Game { get; set; }
        public int? Position { get; set; }
        //public int? Points { get; set; }

        [ForeignKey("BowlingRosterID1")]
        public BowlingRoster BowlingRoster1 { get; set; }

        [ForeignKey("BowlingRosterID2")]
        public BowlingRoster BowlingRoster2 { get; set; }

        [NotMapped]
        public string Winner { get; set; }
    }

    public class BowlingGames
    {
        public int TeamID { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Score { get; set; }
        public int Game { get; set; }
        public int Points { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
