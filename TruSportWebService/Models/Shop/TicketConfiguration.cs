using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TicketConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TicketCompanyID { get; set; }
        public int ValidFrom { get; set; }
        public int Stock { get; set; }

        [ForeignKey("TicketCompanyID")]
        public TicketCompany TicketCompany { get; set; }

        //[ForeignKey("TeamID")]
        //public Team Team { get; set; }
    }
}
