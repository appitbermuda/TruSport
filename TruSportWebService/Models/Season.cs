using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Season
    {
        public string ID { get; set; }
        public int Key { get; set; }
        public string Date { get; set; }
        public bool IsCurrent { get; set; }

    }
}
