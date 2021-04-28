using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TournamentMatchType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisTournamentID { get; set; }
        public string MatchTypeID { get; set; }

        [ForeignKey("TennisTournamentID")]
        public TennisTournament Tournament { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }
    }
}
