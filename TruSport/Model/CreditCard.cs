using System;
using SQLite;

namespace TruSport.Model
{
    public class CreditCard
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string Last4 { get; set; }
        public string CVV { get; set; }
        public string Expiry { get; set; }
        public bool IsDefault { get; set; }
    }
}
