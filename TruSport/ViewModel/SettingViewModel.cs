using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private ObservableCollection<string> _sportCollection;
        private string _defaultSport;
        private bool _footballAlert;
        private bool _cricketAlert;
        private bool _favouriteAlert;
        private bool _isActivityIndicatorVisible;
        INavigation Navigation;

        SportService sportService;

        public SettingViewModel()
        {
            sportService = new SportService();
            SportCollection = new ObservableCollection<string>();

            GenerateSource();

        }

        public SettingViewModel(INavigation navigation)
        {
            Navigation = navigation;
            GenerateSource();
        }

        public Command<string> SelectedSportCommand { get; }

        public ObservableCollection<string> SportCollection
        {
            get { return _sportCollection; }
            set { Set(ref _sportCollection, value); }
        }

        public string DefaultSport
        {
            get { return _defaultSport; }
            set { Set(ref _defaultSport, value); }
        }

        public bool FootballAlert
        {
            get { return _footballAlert; }
            set
            {
                Set(ref _footballAlert, value);
                UpdateFootballAlert();
            }
        }

        public bool CricketAlert
        {
            get { return _cricketAlert; }
            set
            {
                Set(ref _cricketAlert, value);
                UpdateCricketAlert();
            }
        }

        public bool FavouriteAlert
        {
            get { return _favouriteAlert; }
            set
            {
                Set(ref _favouriteAlert, value);
                UpdateFavouriteAlert();
            }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                //var sport = await SecureStorage.GetAsync("DefaultSport");
                var defaultSport = await App.Database.GetDefaultSport();

                if (defaultSport == null || defaultSport.Sport == "Football")
                {
                        var sport = await App.Database.GetSport("Football");
                        //await SecureStorage.SetAsync("DefaultSport", sport);

                        //Sport football = await App.Database.GetSport(sport);
                        await App.Database.SetDefaultSport(sport);

                }

                DefaultSport = defaultSport.Sport;

                var sportsAlerts = await App.Database.GetAlertSettings();

                //var cricketAlert = await SecureStorage.GetAsync("CricketAlert");

                if (sportsAlerts == null || sportsAlerts.Count == 0)
                {
                    CricketAlert = true;
                    FootballAlert = true;
                    FavouriteAlert = true;
                }
                else
                { 
                    var cricketAlert = sportsAlerts.FirstOrDefault(e => e.Sport.ToLower() == "cricket").IsAlert;

                    CricketAlert = cricketAlert;

                    var footballAlert = sportsAlerts.FirstOrDefault(e => e.Sport.ToLower() == "football").IsAlert;

                    FootballAlert = footballAlert;

                    //var favouriteAlert = sportsAlerts.FirstOrDefault(e => e.Sport.ToLower() == "favourite").IsAlert;

                    //if (favouriteAlert == null)
                    //    favouriteAlert = "false";

                    //FavouriteAlert = Convert.ToBoolean(favouriteAlert);
                }

                var sports = await sportService.GetSports();
                SportCollection = new ObservableCollection<string>(sports.Select(e => e.Name));


            }
            catch(Exception ex)
            {

            }

            IsActivityIndicatorVisible = false;
        }

        public async Task UpdateCricketAlert()
        {
            try
            {
                NotiAlert alert = new NotiAlert
                {
                    Sport = "Cricket",
                    IsAlert = CricketAlert
                };

                await App.Database.SaveAlertSetting(alert);
                await SecureStorage.SetAsync("CricketAlert", CricketAlert.ToString());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket Alert");
            }
        }

        public async Task UpdateFootballAlert()
        {
            try
            {
                NotiAlert alert = new NotiAlert
                {
                    Sport = "Football",
                    IsAlert = FootballAlert
                };

                await App.Database.SaveAlertSetting(alert);
                await SecureStorage.SetAsync("FootballAlert", FootballAlert.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Football Alert");
            }
        }

        public async Task UpdateFavouriteAlert()
        {
            try
            {
                await SecureStorage.SetAsync("FavouriteAlert", FavouriteAlert.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Favourite Alert");
            }
        }
    }
}
