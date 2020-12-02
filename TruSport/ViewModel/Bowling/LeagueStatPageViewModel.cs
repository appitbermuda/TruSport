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
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels.Bowling
{
    public class LeagueStatPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueStat tappedInfo; 
        private ObservableCollection<LeagueStat> _playerSeasonHG;
        private ObservableCollection<LeagueStat> _playerSeasonHS;
        private ObservableCollection<LeagueStat> _teamSeasonHG;
        private ObservableCollection<LeagueStat> _teamSeasonHS;

        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        private Ad _ad;

        LeagueStatService leagueStatsService;
        AdService adService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            PlayerSeasonHGCollection = new ObservableCollection<LeagueStat>();
            PlayerSeasonHSCollection = new ObservableCollection<LeagueStat>();
            TeamSeasonHGCollection = new ObservableCollection<LeagueStat>();
            TeamSeasonHSCollection = new ObservableCollection<LeagueStat>();

            leagueStatsService = new LeagueStatService();
            adService = new AdService();
            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
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

        public ObservableCollection<LeagueStat> PlayerSeasonHGCollection
        {
            get { return _playerSeasonHG; }
            set { Set(ref this._playerSeasonHG, value); }
        }

        public ObservableCollection<LeagueStat> PlayerSeasonHSCollection
        {
            get { return _playerSeasonHS; }
            set { Set(ref this._playerSeasonHS, value); }
        }

        public ObservableCollection<LeagueStat> TeamSeasonHGCollection
        {
            get { return _teamSeasonHG; }
            set { Set(ref this._teamSeasonHG, value); }
        }

        public ObservableCollection<LeagueStat> TeamSeasonHSCollection
        {
            get { return _teamSeasonHS; }
            set { Set(ref this._teamSeasonHS, value); }
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

        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
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
                    await Task.Run(async () =>
                    {
                        var ads = await adService.GetAds();

                        if (ads != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Ad = ads.Any(e => e.Sport == Constants.Bowling) ? ads.FirstOrDefault(e => e.Sport == Constants.Bowling) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var playerSeasonHG = await leagueStatsService.GetBowlingSeasonHG();

                    if(playerSeasonHG != null)
                        PlayerSeasonHGCollection = new ObservableCollection<LeagueStat>(playerSeasonHG);

                    var playerSeasonHS = await leagueStatsService.GetBowlingSeasonHS();

                    if (playerSeasonHS != null)
                        PlayerSeasonHSCollection = new ObservableCollection<LeagueStat>(playerSeasonHS);

                    var teamSeasonHG = await leagueStatsService.GetBowlingSeasonTeamHG();

                    if (teamSeasonHG != null)
                        TeamSeasonHGCollection = new ObservableCollection<LeagueStat>(teamSeasonHG);

                    var teamSeasonHS = await leagueStatsService.GetBowlingSeasonTeamHS();

                    if (teamSeasonHS != null)
                        TeamSeasonHSCollection = new ObservableCollection<LeagueStat>(teamSeasonHS);

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

        private async void AdTapped()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await adService.Impressions(Ad.ID);
                });

                await Launcher.OpenAsync(new Uri(Ad.URL));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad Tapped");
            }
        }

        #endregion
    }
}
