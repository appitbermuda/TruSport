using System;
using SQLite;

namespace TruSport.Model
{
    public class MatchTicket
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }

        [Ignore]
        public virtual Order Order { get; set; }

        [Ignore]
        public virtual FixtureProduct FixtureProduct { get; set; }

        [Ignore]
        public string CustomerTicket { get; set; }

        [Ignore]
        public virtual CustomerMatchTicket CustomerMatchTicket { get; set; }

    }
}
