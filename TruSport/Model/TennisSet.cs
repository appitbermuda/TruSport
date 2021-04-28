using System;
using System.Collections.ObjectModel;
using SQLite;

namespace TruSport.Model
{
    public class TennisSet
    {
        public string ID { get; set; }
        public string TennisMatchID { get; set; }
        public int? Score { get; set; }
        public int? Set { get; set; }

        public TennisMatch TennisMatch { get; set; }

        [Ignore]
        public virtual ObservableCollection<TennisGame> TennisGames { get; set; }

        [Ignore]
        public virtual TennisTiebreak TennisTiebreak { get; set; }
    }
}
