using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using OnTrackWebService.Models.Shop;

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

    public class RegisterTicketCompany
    {
        public string SportID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Logo { get; set; }

        public List<Product> Products { get; set; }
    }
}
