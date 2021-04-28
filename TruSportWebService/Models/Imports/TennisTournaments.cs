using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Imports
{
    public class TennisTournaments
    {
        public string Name { get; set; }
        public string FieldID { get; set; }
        public string CourtTypeID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int? Rounds { get; set; }

        public List<MatchType> MatchTypes { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
