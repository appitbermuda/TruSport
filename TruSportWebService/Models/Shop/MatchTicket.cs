using System;
using System.Collections.Generic;

namespace OnTrackWebService.Models.Shop
{
    public class MatchTicket
    {
        public virtual Fixture Fixture { get; set; }
        public virtual Product Product { get; set; }
    }
}
