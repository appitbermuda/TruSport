using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruSport.Views.Tickets;

namespace TruSport.Views.Tickets
{
    public class TicketMasterDetailPageMenuItem
    {
        public TicketMasterDetailPageMenuItem()
        {
            TargetType = typeof(TicketPurchasePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }

        public object[] args { get; set; }
    }
}
