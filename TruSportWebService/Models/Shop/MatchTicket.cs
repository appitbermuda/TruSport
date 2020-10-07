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
        public bool Validated { get; set; }
        public DateTime? ValidatedTime { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        //[ForeignKey("FixtureID")]
        //public virtual Fixture Fixture { get; set; }

        [ForeignKey("FixtureProductID")]
        public virtual FixtureProduct FixtureProduct { get; set; }

        [NotMapped]
        public virtual CustomerMatchTicket CustomerMatchTicket { get; set; }

    }
}
