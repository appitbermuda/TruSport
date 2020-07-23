using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class ProductType
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
    }
}
