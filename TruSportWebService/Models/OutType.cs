using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class OutType
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
    }
}
