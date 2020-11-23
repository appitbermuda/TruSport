using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models.Ad
{
    public class Ad
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Sport { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
