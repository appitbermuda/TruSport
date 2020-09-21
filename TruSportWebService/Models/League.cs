using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class League
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string SportID { get; set; }
        public int Order { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
