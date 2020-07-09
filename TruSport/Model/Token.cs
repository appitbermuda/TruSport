using System;
using SQLite;

namespace TruSport.Model
{
    public class Token
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        public string TokenId { get; set; }
    }
}
