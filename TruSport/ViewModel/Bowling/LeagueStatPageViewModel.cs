using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TruSport.Services;
using Xamarin.Essentials;
using System.Diagnostics;

namespace TruSport.ViewModels.Bowling
{
    public class LeagueStatPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueStat tappedInfo; 
        private ObservableCollection<LeagueStat> _playerMostPinsCollection;
        private ObservableCollection<LeagueStat> _playerMostWicketsCollection;
        
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueStatService leagueStatsService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            PlayerMostPinsCollection = new ObservableCollection<LeagueStat>();
            PlayerMostWicketsCollection = new ObservableCollection<LeagueStat>();

            leagueStatsService = new LeagueStatService();
            GenerateSource();
        }

        #endregion

        #region Properties
        internal SfListView PlayerCategoryList
        {
            get;
            set;
        }
        internal SfListView AgentCategoryList
        {
            get;
            set;
        }
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }
        public Command<object> FavoriteTapCommand
        {
            get { return favoriteTapCommand; }
            set { favoriteTapCommand = value; }
        }
        public Command<object> ResetTapCommand
        {
            get { return resetTapCommand; }
            set { resetTapCommand = value; }
        }

        public ObservableCollection<LeagueStat> PlayerMostPinsCollection
        {
            get { return _playerMostPinsCollection; }
            set { Set(ref this._playerMostPinsCollection, value); }
        }

        public ObservableCollection<LeagueStat> PlayerMostWicketsCollection
        {
            get { return _playerMostWicketsCollection; }
            set { Set(ref this._playerMostWicketsCollection, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            //for(var i = 0; i < SyncTitles.Length; i++)
            //{
            //    SyncTitleCollection.Add(SyncTitles[i]);
            //}
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                try
                {
                    var pinsByPlayer = await leagueStatsService.GetMostPinsByPlayer();

                    if(pinsByPlayer != null)
                        PlayerMostPinsCollection = new ObservableCollection<LeagueStat>(pinsByPlayer);

                    //var wicketsByPlayer = await leagueStatsService.GetMostWicketsByPlayer();

                    //if(wicketsByPlayer != null)
                    //    PlayerMostWicketsCollection = new ObservableCollection<LeagueStat>(wicketsByPlayer);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "League Stats");
                }
            }
            else
                NoConnectivity = true;
            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        #endregion
    }
}
