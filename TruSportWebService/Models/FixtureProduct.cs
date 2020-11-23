using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Models
{
    public class FixtureProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string FixtureID { get; set; }
        public string ProductID { get; set; }
        public DateTime? ValidFrom { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
