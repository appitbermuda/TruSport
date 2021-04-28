using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisTournament
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string FieldID { get; set; }
        //public string LeagueID { get; set; }
        public string CourtTypeID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NoOfRounds { get; set; }


        [ForeignKey("FieldID")]
        public Field Field { get; set; }
        //public League League { get; set; }

        [ForeignKey("CourtTypeID")]
        public CourtType CourtType { get; set; }

    }
}
