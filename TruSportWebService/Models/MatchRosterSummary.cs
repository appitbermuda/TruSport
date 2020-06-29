using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class MatchRosterSummary
    {

        public string ID { get; set; }
        public string MatchRosterID { get; set; }
        public string FixtureID { get; set; }
        public string TeamID { get; set; }
        public string PlayerID { get; set; }
        public string SubstitutePlayerID { get; set; }
        public string AssistPlayerID { get; set; }
        public int Goal { get; set; }
        public int Assist { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public bool IsSub { get; set; }
        public bool IsHomeTeam { get; set; }
        public int Minute { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        [ForeignKey("SubstitutePlayerID")]
        public Player SubsitutePlayer { get; set; }

        [ForeignKey("AssistPlayerID")]
        public Player AssistPlayer { get; set; }
    }
}
