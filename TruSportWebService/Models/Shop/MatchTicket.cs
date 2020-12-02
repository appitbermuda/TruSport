using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class MatchTicket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string FixtureProductID { get; set; }
        public string TransferCustomerID { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }
        public bool? IsTransfer { get; set; }
        //public bool TransferRejected { get; set; }
        public DateTime? TransferTime { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("TransferCustomerID")]
        public virtual Customer TransferCustomer { get; set; }

        [ForeignKey("FixtureProductID")]
        public virtual FixtureProduct FixtureProduct { get; set; }

        [NotMapped]
        public virtual CustomerMatchTicket CustomerMatchTicket { get; set; }

    }
}
