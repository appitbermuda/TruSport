using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Fixture
    {
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsPostPoned { get; set; }

        [ForeignKey("HomeTeamID")]
        public Team HomeTeam { get; set; }

        [ForeignKey("AwayTeamID")]
        public Team AwayTeam { get; set; }

        [ForeignKey("FieldID")]
        public Field Field { get; set; }

        [ForeignKey("LeagueID")]
        public League League { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }

        [ForeignKey("SeasonID")]
        public Season Season { get; set; }

        public virtual Match Match { get; set; }
        public virtual List<MatchRoster> MatchRosters { get; set; }
    }
}
