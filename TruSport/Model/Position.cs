using System;
using SQLite;

namespace TruSport.Model
{
    public class Position
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SportID { get; set; }

        [Ignore]
        public Sport Sport { get; set; }
    }
}
