using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TicketScanner
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
