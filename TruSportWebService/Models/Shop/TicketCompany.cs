using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OnTrackWebService.Models
{
    public class TicketCompany
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string SportID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Logo { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
