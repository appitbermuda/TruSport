using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using TruSport.Services;
using TruSport.Views.Football;
using Syncfusion.DataSource.Extensions;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace TruSport.ViewModels
{
    public class TeamProfilePageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Transfers> transferCollection;
        private TeamListView tappedInfo;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<Fixture> fixtureCollection;
        private ObservableCollection<Fixture> formCollection;
        private ObservableCollection<Coach> coachCollection;
        private ObservableCollection<PlayerSeason> playerCollection;
        private ObservableCollection<LeagueTable> tableCollection;
        private LeagueTable tableItem;
        private Team teamItem;
        private Field field;
        private string _coach;
        private Command<object> leagueSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool noTransfers;

        TeamService teamService;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        PlayerService playerService;
        INavigation Navigation;
        TransferService transferService;
        AdService adService;

        #endregion

        #region Constructor

        public TeamProfilePageViewModel(INavigation navigation, Team Team)
        {
            Navigation = navigation;
            FixtureCollection = new ObservableCollection<Fixture>();
            FormCollection = new ObservableCollection<Fixture>();
            PlayerCollection = new ObservableCollection<PlayerSeason>();
            TableCollection = new ObservableCollection<LeagueTable>();
            CoachCollection = new ObservableCollection<Coach>();
            TransferCollection = new ObservableCollection<Transfers>();
            TableItem = new LeagueTable();
            TeamItem = new Team();

            SyncTitleCollection = new ObservableCollection<string>();

            transferService = new TransferService();
            teamService = new TeamService();
            leagueTableService = new LeagueTableService();
            fixtureService = new FixtureService();
            playerService = new PlayerService();
            adService = new AdService();

            GenerateSource(Team);

            AdTappedCommand = new Command(AdTapped);
            FavouriteCommand = new Command(async () => await Favourite());
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
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
        public Command<object> LeagueSelectedCommand { get; }
        public Command FavouriteCommand { get; }
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

        public ObservableCollection<Transfers> TransferCollection
        {
            get { return transferCollection; }
            set { Set(ref transferCollection, value); }
        }

        public ObservableCollection<Fixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public ObservableCollection<Fixture> FormCollection
        {
            get { return formCollection; }
            set { Set(ref formCollection, value); }
        }

        public ObservableCollection<PlayerSeason> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public ObservableCollection<LeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<Coach> CoachCollection
        {
            get { return coachCollection; }
            set { Set(ref coachCollection, value); }
        }

        public LeagueTable TableItem
        {
            get { return tableItem; }
            set { Set(ref tableItem, value); }
        }

        public Team TeamItem
        {
            get { return teamItem; }
            set { Set(ref teamItem, value); }
        }

        public Field Field
        {
            get { return field; }
            set { Set(ref field, value); }
        }

        public string Coach
        {
            get { return _coach; }
            set { Set(ref _coach, value); }
        }

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { Set(ref syncTitleCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsFavourite
        {
            get { return isFavourite; }
            set { Set(ref isFavourite, value); }
        }

        public bool NoTransfers
        {
            get { return noTransfers; }
            set { Set(ref noTransfers, value); }
        }

        #endregion

        #region Generate Source


        async Task GenerateSource(Team Team)
        {
            IsActivityIndicatorVisible = true;

            try
            {
                TeamItem = await teamService.GetFootballProfile(Team.ID);

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

                IsFavourite = await App.Database.IsTeamFavourite(Team.ID);

                if (TeamItem.Coaches != null && TeamItem.Coaches.Count > 0)
                    Coach = TeamItem.Coaches.FirstOrDefault().Name;

                var fixtures = await fixtureService.GetFootballTeamFixtures(Team.ID);
                if (fixtures != null)
                    FixtureCollection = new ObservableCollection<Fixture>(fixtures.OrderBy(e => e.Date));


                var form = await fixtureService.GetFootballTeamForm(Team.ID);

                if (form != null)
                    FormCollection = new ObservableCollection<Fixture>(form);

                //if (Team.Coaches != null)
                //    CoachCollection = new ObservableCollection<Coach>(Team.Coaches);

                var players = await playerService.GetTeamPlayers(Team.ID);
                if (players != null)
                    PlayerCollection = new ObservableCollection<PlayerSeason>(players.Where(e => e.Season.IsCurrent).OrderBy(e => e.Player.LastName));

                //if (_team.Fixtures != null)
                //    FixtureCollection = new ObservableCollection<Fixture>(_team.Fixtures.OrderBy(e => e.Date));

                //if (_team.Form != null)
                //    FormCollection = new ObservableCollection<Fixture>(_team.Form);

                //List<LeagueTable> tables = new List<LeagueTable>();

                var table = await leagueTableService.GetFootballTeamLeagueTable(Team.ID);

                if (table != null)
                {
                    TableCollection = new ObservableCollection<LeagueTable>(table);


                    TableItem = TableCollection.FirstOrDefault(e => e.IsSelectedTeam);
                }
                //}

                if (TeamItem.TeamSeasons.FirstOrDefault().Transfers.Count > 0)
                    NoTransfers = false;
                else
                    NoTransfers = true;
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Team Profile");
            }

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        private async void SelectedLeague(object obj)
        {
            try
            {
                var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

                var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
                var data = items[0];

                var league = data.League;

                await Navigation.PushAsync(new CompetitionDetailsPage(league));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Select League");
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

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteTeamFavourite(TeamItem.ID);
                    //await App.Current.MainPage.DisplayAlert("Success", "Favourite deleted successfully.", "OK");
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        TeamID = TeamItem.ID,
                        Type = "Team"
                    };

                    await App.Database.SaveFootballFavourite(favourite);
                    //await App.Current.MainPage.DisplayAlert("Success", "Favourite saved successfully.", "OK");
                }

                //await DisplayAlert("Error", "There was an issue saving this transfer, please try again or contact us.", "OK");


            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }


        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as TeamListView;
        }

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
