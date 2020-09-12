using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Shop
{
    public class ProductType
    {
        public string ID { get; set; }
        public string SportID { get; set; }
        public string MatchTypeID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        [ForeignKey("SportID")]
        public Sport Sport { get; set; }

        [ForeignKey("MatchTypeID")]
        public MatchType MatchType { get; set; }
    }
}
