using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TicketResponse
    {
        public List<string> Tickets { get; set; }
        public string Response { get; set; }
        public bool IsValidated { get; set; }
    }
}
