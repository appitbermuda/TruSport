using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class TicketMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string TicketCompanyID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        [ForeignKey("TicketCompanyID")]
        public TicketCompany TicketCompany { get; set; }
    }

    public class TicketMembers
    {
        public string Team { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public string Exception { get; set; }
    }
}
