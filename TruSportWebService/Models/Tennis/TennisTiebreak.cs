using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisTiebreak
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisSetID { get; set; }
        public int? Score { get; set; }

        public TennisSet Set { get; set; }
    }
}
