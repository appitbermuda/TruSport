using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string TemporaryPassword { get; set; }
        public bool IsValidated { get; set; }

        [NotMapped]
        public string Token { get; set; }

        [NotMapped]
        public virtual List<Order> Orders { get; set; }

        [NotMapped]
        public string Name => FirstName + " " + LastName;
    }

    public class CustomerResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
