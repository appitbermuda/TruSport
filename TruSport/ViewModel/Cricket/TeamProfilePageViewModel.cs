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
using Microsoft.AppCenter.Crashes;
using System.Diagnostics;

namespace TruSport.ViewModels.Cricket
{
    public class TeamProfilePageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Transfers> transferCollection;
        private TeamListView tappedInfo;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<CricketFixture> fixtureCollection;
        private ObservableCollection<CricketFixture> formCollection;
        private ObservableCollection<Coach> coachCollection;
        private ObservableCollection<CricketPlayerSeason> playerCollection;
        private ObservableCollection<CricketLeagueTable> tableCollection;
        private CricketLeagueTable tableItem;
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

        #endregion

        #region Constructor

        public TeamProfilePageViewModel(INavigation navigation, Team Team)
        {
            Navigation = navigation;
            FixtureCollection = new ObservableCollection<CricketFixture>();
            FormCollection = new ObservableCollection<CricketFixture>();
            PlayerCollection = new ObservableCollection<CricketPlayerSeason>();
            TableCollection = new ObservableCollection<CricketLeagueTable>();
            CoachCollection = new ObservableCollection<Coach>();
            TransferCollection = new ObservableCollection<Transfers>();
            TableItem = new CricketLeagueTable();
            TeamItem = new Team();

            SyncTitleCollection = new ObservableCollection<string>();

            transferService = new TransferService();
            teamService = new TeamService();
            leagueTableService = new LeagueTableService();
            fixtureService = new FixtureService();
            playerService = new PlayerService();
            GenerateSource(Team);

            FavouriteCommand = new Command(async () => await Favourite());
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
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
        public Command<object> LeagueSelectedCommand
        {
            get { return leagueSelectedCommand; }
            set { leagueSelectedCommand = value; }
        }
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

        public ObservableCollection<CricketFixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public ObservableCollection<CricketFixture> FormCollection
        {
            get { return formCollection; }
            set { Set(ref formCollection, value); }
        }

        public ObservableCollection<CricketPlayerSeason> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public ObservableCollection<CricketLeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<Coach> CoachCollection
        {
            get { return coachCollection; }
            set { Set(ref coachCollection, value); }
        }

        public CricketLeagueTable TableItem
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


        internal async void GenerateSource(Team Team)
        {
            IsActivityIndicatorVisible = true;

            try
            {

                var _team = await teamService.GetCricketProfile(Team.ID);

                TeamItem = _team;

                IsFavourite = await App.Database.IsTeamFavourite(Team.ID);

                if(TeamItem.Coaches != null && TeamItem.Coaches.Count > 0)
                    Coach = TeamItem.Coaches.FirstOrDefault().Name;
                //if (Team.Coaches != null)
                //    CoachCollection = new ObservableCollection<Coach>(Team.Coaches);

                ////var players = await playerService.GetTeamPlayers(Team.ID);
                ////PlayerCollection = new ObservableCollection<PlayerSeason>(players.OrderBy(e => e.Player.LastName));

                //if(_team.TeamSeasons[0].Players != null)
                //    PlayerCollection = new ObservableCollection<PlayerSeason>(_team.Players.OrderBy(e => e.Player.LastName));

                var players = await playerService.GetCricketTeamPlayers(Team.ID);
                if (players != null)
                    PlayerCollection = new ObservableCollection<CricketPlayerSeason>(players.Where(e => e.Season.IsCurrent).OrderBy(e => e.Player.LastName));


                var fixtures = await fixtureService.GetCricketTeamFixtures(Team.ID);
                if(fixtures != null)
                    FixtureCollection = new ObservableCollection<CricketFixture>(fixtures.OrderBy(e => e.Date));

                //if (_team.Fixtures != null)
                //    FixtureCollection = new ObservableCollection<CricketFixture>(_team.Fixtures.OrderBy(e => e.Date));

                var form = await fixtureService.GetCricketTeamForm(Team.ID);

                if (form != null)
                    FormCollection = new ObservableCollection<CricketFixture>(form);

                //if (_team.Form != null)
                //    FormCollection = new ObservableCollection<CricketFixture>(_team.Form);

                //List<CricketLeagueTable> tables = new List<CricketLeagueTable>();

                var table = await leagueTableService.GetCricketTeamLeagueTable(Team.ID);

                if (table != null)
                {
                    TableCollection = new ObservableCollection<CricketLeagueTable>(table);


                    TableItem = TableCollection.FirstOrDefault(e => e.IsSelectedTeam);
                }
                //TableItem = TeamItem.TeamSeasons.FirstOrDefault(e => e.Season.IsCurrent).CricketTable.FirstOrDefault(e => e.TeamID == Team.ID);
                //}

                //var transfers = await transferService.GetTransfers();
                //transfers = transfers.Where(e => e.NewTeam == Team.Name).ToList();

                if (TeamItem.TeamSeasons.FirstOrDefault(e => e.Season.IsCurrent).Transfers?.Count > 0)
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
        }

        private async void SelectedLeague(object obj)
        {
            try
            {
                var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

                var items = new List<CricketFixture>(groupResult.Items.ToList<CricketFixture>());
                var data = items[0];

                var league = data.League;

                await Navigation.PushAsync(new CompetitionDetailsPage(league));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Select League");
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
                }
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
