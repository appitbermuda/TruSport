using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class Inventory
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public int Stock { get; set; }

        [Ignore]
        public Product Product { get; set; }
    }
}
