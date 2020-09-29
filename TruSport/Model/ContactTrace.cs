using System;
using SQLite;

namespace TruSport.Model
{
    public class ContactTrace
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [Ignore]
        public Order Order { get; set; }
    }
}
