using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class TicketCompanyUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TicketCompanyID { get; set; }
        public string UserID { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("TicketCompanyID")]
        public TicketCompany TicketCompany { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
