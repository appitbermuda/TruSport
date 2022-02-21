using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Ticket
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Email { get; set; }
        public string TicketCompanyID { get; set; }

        [ForeignKey("TicketCompanyID")]
        public TicketCompany TicketCompany { get; set; }
    }
}
