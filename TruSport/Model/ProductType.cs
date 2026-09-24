using System;
using SQLite;

namespace TruSport.Model
{
    public class ProductType
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string SportID { get; set; }
        public string MatchTypeID { get; set; }

        [Ignore]
        public Sport Sport { get; set; }

        [Ignore]
        public MatchType MatchType { get; set; }
    }
}
