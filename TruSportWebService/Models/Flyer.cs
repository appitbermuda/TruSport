using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Flyer
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SportID { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }
    }
}
