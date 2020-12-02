using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class BowlingRoster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string BowlingFixtureID { get; set; }
        public string TeamID { get; set; }
        public string BowlingPlayerSeasonID { get; set; }
        public int? Position { get; set; }

        [ForeignKey("BowlingFixtureID")]
        public BowlingFixture BowlingFixture { get; set; }

        [ForeignKey("TeamID")]
        public BowlingTeam Team { get; set; }

        [ForeignKey("BowlingPlayerSeasonID")]
        public BowlingPlayerSeason BowlingPlayerSeason { get; set; }


        public List<BowlingGame> BowlingGames { get; set; }

        [NotMapped]
        public List<BowlingGameResult> BowlingGameResults { get; set; }
    }

    public class BowlingRosterListView
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomePlayerName { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayPlayerName { get; set; }
        public int Position { get; set; }
    }

    public class BowlingRosters
    {
        public string BowlingFixtureID { get; set; }
        public int TeamID { get; set; }
        public string BowlingPlayerSeasonID { get; set; }
        public int Position { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
