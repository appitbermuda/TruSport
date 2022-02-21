using System;
using SQLite;

namespace TruSport.Model.Ticket
{
    public class Member
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string Email { get; set; }
        public string TicketCompanyID { get; set; }

        [Ignore]
        public TicketCompany TicketCompany { get; set; }
    }
}
