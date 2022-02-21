using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Models
{
    public class RegisterSportEvent
    {
        public string TicketCompanyID { get; set; }
        public string HomeTeamID { get; set; }
    }
}
