using System;
using SQLite;

namespace TruSport.Model
{
    public class FixtureProduct
    {
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }

        [Ignore]
        public Fixture Fixture { get; set; }

        [Ignore]
        public Product Product { get; set; }
    }
}
