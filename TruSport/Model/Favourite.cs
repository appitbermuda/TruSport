using System;
using SQLite;

namespace TruSport.Model
{
    [Table("Favourite")]
    public class Favouritess
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

    }
}
