using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class MatchStat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string MatchRosterID { get; set; }
        public string AssistPlayerID { get; set; }
        public int? Goal { get; set; }
        public int? YellowCard { get; set; }
        public int? RedCard { get; set; }
        public int? GoalTime { get; set; }
        public int? YellowCardTime { get; set; }
        public int? RedCardTime { get; set; }

        [ForeignKey("MatchRosterID")]
        public MatchRoster MatchRoster { get; set; }

        [ForeignKey("AssistPlayerID")]
        public Player AssistPlayer { get; set; }
    }
}
