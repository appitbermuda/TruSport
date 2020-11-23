using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsSelectable { get; set; }
        public bool RequiresTeam { get; set; }
        public string SportID { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
