using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class TicketCompany
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string SportID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Logo { get; set; }

        [Ignore]
        public Sport Sport { get; set; }
    }
}
