using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using TruSport.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using System.Diagnostics;

namespace TruSport.ViewModels.Cricket
{
    public class TeamPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Team> teamCollection;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;

        TeamService teamService;
        AdService adService;

        #endregion

        #region Constructor

        public TeamPageViewModel()
        {
            TeamCollection = new ObservableCollection<Team>();

            teamService = new TeamService();
            adService = new AdService();

            GenerateSource();

            TeamFavouriteCommand = new Command<object>(TeamFavourite);
            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command<object> TeamFavouriteCommand { get; }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

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

                    var teamFavourites = await App.Database.GetCricketTeamFavourites();

                    var teams = await teamService.GetCricketTeams();

                    if(teamFavourites != null && teamFavourites.Count > 0)
                        teams.ForEach(e => e.IsFavourite = teamFavourites.Any(d => d.TeamID == e.ID));

                    TeamCollection = new ObservableCollection<Team>(teams);

                    
                }
                else
                    NoConnectivity = true;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hmmmm", "Looks like something went wrong, please check you are connected to a wifi or cellular connection.", "Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
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

        public async void TeamFavourite(object obj)
        {
            try
            {
                var team = obj as Team;

                var item = TeamCollection.IndexOf(team);
                if (item >= 0)
                {
                    team.IsFavourite = !team.IsFavourite;
                    TeamCollection[item] = team;

                    if (!TeamCollection[item].IsFavourite)
                    {
                        await App.Database.DeleteTeamFavourite(team.ID);
                    }
                    else
                    {

                        var favourite = new Favourite
                        {
                            TeamID = team.ID,
                            Type = "Team"
                        };

                        await App.Database.SaveCricketFavourite(favourite);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


        #region Player Info

        TeamListView[] Player = new TeamListView[]
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
