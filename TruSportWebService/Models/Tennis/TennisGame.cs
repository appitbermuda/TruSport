using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisGame
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisSetID { get; set; }
        public int? Score { get; set; }
        public int? Game { get; set; }
        public int? Point { get; set; }

        public TennisSet TennisSet { get; set; }
    }
}
