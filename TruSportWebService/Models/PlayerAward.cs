using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class Award
    {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public string AwardTypeID { get; set; }
        public string SeasonID { get; set; }
        public string SportID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual Player Player { get; set; }
        public virtual AwardType AwardType { get; set; }
        public virtual Season Season { get; set; }
        public virtual Sport Sport { get; set; }
    }
}
