using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnTrackWebService.Models
{
    public class TennisSet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string TennisMatchID { get; set; }
        public int? Score { get; set; }
        public int? Set { get; set; }

        [ForeignKey("TennisMatchID")]
        public TennisMatch TennisMatch { get; set; }

        //[NotMapped]
        public virtual ObservableCollection<TennisGame> TennisGames { get; set; }

        //[NotMapped]
        public virtual TennisTiebreak TennisTiebreak { get; set; }
    }
}
