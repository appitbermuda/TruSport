using System;
using SQLite;

namespace TruSport.Model
{
    public class NotiAlert
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        public string Sport { get; set; }
        public bool IsAlert { get; set; }
    }
}
