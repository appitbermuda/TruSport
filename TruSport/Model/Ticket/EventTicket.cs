using System;
using SQLite;

namespace TruSport.Model
{
    public class EventTicket
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string SportEventID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }

        [Ignore]
        public SportEvent SportEvent { get; set; }

        [Ignore]
        public Product Product { get; set; }
    }
}
