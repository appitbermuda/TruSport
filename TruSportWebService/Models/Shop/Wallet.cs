using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class Wallet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string TokenPAN { get; set; }
        public string Expiry { get; set; }
        public string IsDefault { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
    }
}
