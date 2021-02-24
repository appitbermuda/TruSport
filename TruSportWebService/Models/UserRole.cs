using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string UserID { get; set; }
        public string AdminIdentifier { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
