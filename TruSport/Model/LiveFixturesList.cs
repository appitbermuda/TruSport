using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TruSport.Model
{
    public class LiveFixturesList : INotifyPropertyChanged
    {
        private string _gameTime;

        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public string FieldID { get; set; }
        public string LeagueID { get; set; }
        public string MatchTypeID { get; set; }
        public string SeasonID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }

        public Field Field { get; set; }

        public League League { get; set; }

        public Season Season { get; set; }

        public MatchType MatchType { get; set; }

        public virtual Match Match { get; set; }
        public virtual List<MatchRoster> MatchRosters { get; set; }
        
        //public string FixtureID { get; set; }
        //public int HomeTeamScore { get; set; }
        //public int AwayTeamScore { get; set; }
        //public string HomeTeamName { get; set; }
        //public string AwayTeamName { get; set; }
        //public string HomeTeamLogo { get; set; }
        //public string AwayTeamLogo { get; set; }
        //public string FieldName { get; set; }
        //public string LeagueName { get; set; }
        //public string MatchTypeName { get; set; }

        public string GameTime
        {
            get { return _gameTime; }
            internal set
            {
                _gameTime = value;
                OnPropertyChanged("GameTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LiveFixture
    {
        public string ID { get; set; }
        public string HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamAlias { get; set; }
        public string HomeTeamLogo { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? HomeTeamPenalty { get; set; }
        public string AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamAlias { get; set; }
        public string AwayTeamLogo { get; set; }
        public int? AwayTeamScore { get; set; }
        public int? AwayTeamPenalty { get; set; }
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public string LeagueID { get; set; }
        public string LeagueName { get; set; }
        public string MatchTypeID { get; set; }
        public string MatchTypeName { get; set; }
        public string SeasonID { get; set; }
        public string SeasonDate { get; set; }
        public bool IsPenalties { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public string GameTime { get; set; }
    }
}
