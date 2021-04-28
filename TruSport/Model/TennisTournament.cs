using System;
namespace TruSport.Model
{
    public class TennisTournament
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string CourtTypeID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NoOfRounds { get; set; }

        public Field Field { get; set; }
        public League League { get; set; }
        public CourtType CourtType { get; set; }

    }
}
