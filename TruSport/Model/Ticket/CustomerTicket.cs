using System;
using SQLite;

namespace TruSport.Model
{
    public class CustomerTicket
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string EventTicketID { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }
        public bool IsMemberTicket { get; set; }

        [Ignore]
        public virtual Order Order { get; set; }

        [Ignore]
        public virtual EventTicket EventTicket { get; set; }

        [Ignore]
        public string CustomerTicketObject { get; set; }

        [Ignore]
        public virtual CustomerMatchTicket CustomerMatchTicket { get; set; }

    }
}
