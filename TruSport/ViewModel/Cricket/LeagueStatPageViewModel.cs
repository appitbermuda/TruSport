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
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;

namespace TruSport.ViewModels.Cricket
{
    public class LeagueStatPageViewModel : BaseViewModel
    {
        #region Fields
        private MatchType tappedInfo; 
        private ObservableCollection<MatchType> _matchTypeCollection;
        
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        MatchTypeService matchTypeService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            MatchTypeCollection = new ObservableCollection<MatchType>();

            matchTypeService = new MatchTypeService();
            GenerateSource();
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
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

        public ObservableCollection<MatchType> MatchTypeCollection
        {
            get { return _matchTypeCollection; }
            set { Set(ref this._matchTypeCollection, value); }
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
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                try
                {

                    var matchTypes = await matchTypeService.GetCurrentCricketLeagueTypes();

                    if(matchTypes != null)
                        MatchTypeCollection = new ObservableCollection<MatchType>(matchTypes);
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

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as MatchType;
        }

        #region Player Info

        FixtureListView[] Player = new FixtureListView[]
         {
            
         };

        string[] Agents = new string[]
        {
            "LOCAL",
            "INTERNATIONAL"
        };

        string[] SyncTitles = new string[]
        {
            "First Division",
            "Premier Division",
            "Corona League"
        };

        #endregion
    }
}
