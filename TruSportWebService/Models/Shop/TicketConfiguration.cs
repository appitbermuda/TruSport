using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TicketConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TeamID { get; set; }
        public int ValidFrom { get; set; }
        public int Stock { get; set; }
    }
}
