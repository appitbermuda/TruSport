using System;
using System.Collections.Generic;
using SQLite;

namespace TruSport.Model
{
    public class Customer
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        [Ignore]
        public string Name => FirstName + " " + LastName;

        [Ignore]
        public virtual List<Order> Orders { get; set; }
    }

    public class CustomerRequest
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
