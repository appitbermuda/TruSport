using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Ad
{
    public class Impression
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string AdID { get; set; }
        public int Count { get; set; }

        [ForeignKey("AdID")]
        public Ad Ad { get; set; }
    }
}
