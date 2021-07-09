using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Models
{
    public class EventTicket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string SportEventID { get; set; }
        public string ProductID { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("SportEventID")]
        public SportEvent SportEvent { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
