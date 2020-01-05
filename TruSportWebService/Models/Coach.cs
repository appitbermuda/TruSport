using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Coach
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamID { get; set; }

        [ForeignKey("TeamID")]
        public Team Team { get; set; }
    }
}
