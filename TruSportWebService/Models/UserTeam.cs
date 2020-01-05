using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class UserTeam
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string TeamID { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
