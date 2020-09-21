using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Season
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public int Key { get; set; }
        public string Date { get; set; }
        public bool IsCurrent { get; set; }
        public string SportID { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
