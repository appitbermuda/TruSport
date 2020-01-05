using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Flyer
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
