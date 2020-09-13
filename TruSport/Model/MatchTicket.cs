using System;
namespace TruSport.Model
{
    public class MatchTicket
    {
        public virtual Fixture Fixture { get; set; }
        public virtual Product Product { get; set; }
    }
}
