using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Syncfusion.XForms.Buttons;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MatchRosterViewModel : BaseViewModel
    {

        private Player tappedInfo;
        private Command<object> selectedStarterChanged;
        //private Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs> selectedStarterChanged;
        private Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs> selectedPlayerChanged;
        private Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs> selectedPlayerRemoved;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private ObservableCollection<Player> playerCollection;
        private ObservableCollection<Player> rosterCollection;
        private ObservableCollection<MatchRoster> initialRosterCollection;
        private Fixture fixtureItem;
        private string _teamName;
        private string _teamID;
        private string _fixtureID;
        private int playerCount;

        PlayerService playerService;
        MatchRosterService matchRosterService;
        MatchStatService matchStatService;
        INavigation Navigation;

        private bool _isActivityIndicatorVisible;

        public MatchRosterViewModel(INavigation navigation)
        {
            Navigation = navigation;

            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            InitialRosterCollection = new ObservableCollection<MatchRoster>();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            playerService = new PlayerService();

            GenerateSource();
        }

        public MatchRosterViewModel(INavigation navigation, string fixtureID, string teamID, string teamName)
        {
            Navigation = navigation;
            TeamName = teamName;
            TeamID = teamID;
            FixtureID = fixtureID;

            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            InitialRosterCollection = new ObservableCollection<MatchRoster>();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            playerService = new PlayerService();

            GenerateSource(fixtureID, teamID);

            SaveCommand = new Command(async () => await Save());
            SelectedStarterCommand = new Command<object>(SelectedStarterChanged);
            //SelectedStarterCommand = new Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs>(SelectedStarterChanged);
            SelectedPlayerChangedCommand = new Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs>(SelectedPlayerChanged);
            SelectedPlayerRemovedCommand = new Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs>(SelectedPlayerRemoved);
        }

        public MatchRosterViewModel(INavigation navigation, Fixture fixture, string teamID)
        {
            Navigation = navigation;
            FixtureItem = fixture;
            TeamID = teamID;
            FixtureID = fixture.ID;

            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            InitialRosterCollection = new ObservableCollection<MatchRoster>();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            playerService = new PlayerService();

            GenerateSource(fixture, teamID);

            SaveCommand = new Command(async () => await Save());
            SelectedStarterCommand = new Command<object>(SelectedStarterChanged);
            //SelectedStarterCommand = new Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs>(SelectedStarterChanged);
            SelectedPlayerChangedCommand = new Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs>(SelectedPlayerChanged);
            SelectedPlayerRemovedCommand = new Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs>(SelectedPlayerRemoved);
        }

        public Command SaveCommand { get; }
        public Command<object> SelectedStarterCommand
        {
            get { return selectedStarterChanged; }
            set { selectedStarterChanged = value; }
        }

        //public Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs> SelectedStarterCommand
        //{
        //    get { return selectedStarterChanged; }
        //    set { selectedStarterChanged = value; }
        //}

        public Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs> SelectedPlayerRemovedCommand
        {
            get { return selectedPlayerRemoved; }
            set { selectedPlayerRemoved = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs> SelectedPlayerChangedCommand
        {
            get { return selectedPlayerChanged; }
            set { selectedPlayerChanged = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }

        public ObservableCollection<Player> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public ObservableCollection<MatchRoster> InitialRosterCollection
        {
            get { return initialRosterCollection; }
            set { Set(ref initialRosterCollection, value); }
        }

        public ObservableCollection<Player> RosterCollection
        {
            get { return rosterCollection; }
            set { Set(ref rosterCollection, value); }
        }

        public Fixture FixtureItem
        {
            get { return fixtureItem; }
            set { Set(ref fixtureItem, value); }
        }

        public string TeamName
        {
            get { return _teamName; }
            set { Set(ref _teamName, value); }
        }

        public string TeamID
        {
            get { return _teamID; }
            set { Set(ref _teamID, value); }
        }

        public string FixtureID
        {
            get { return _fixtureID; }
            set { Set(ref _fixtureID, value); }
        }

        public int PlayerCount
        {
            get { return playerCount; }
            set { Set(ref playerCount, value); }
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
                List<PlayerSeason> players = await playerService.GetPlayers();
                PlayerCollection = new ObservableCollection<Player>(players.Where(e => e.Season.IsCurrent).Select(e => e.Player));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(string fixtureID, string teamID)
        {

            IsActivityIndicatorVisible = true;

            try
            {
                PlayerCount = 0;
                List<PlayerSeason> teamPlayers = await playerService.GetTeamPlayers(teamID);
                List<MatchRoster> matchRosterPlayers = await matchRosterService.GetTeamMatchRosters(fixtureID, teamID);

                List<Player> playersList = teamPlayers.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
                List<Player> rosterPlayers = matchRosterPlayers.Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
                List<MatchRoster> initialRosterPlayers = matchRosterPlayers.OrderBy(e => e.IsStarter).ToList();

                var players = playersList.Where(p => !rosterPlayers.Any(p2 => p2.ID == p.ID));
                //get last teamsheet

                PlayerCollection = new ObservableCollection<Player>(players);
                RosterCollection = new ObservableCollection<Player>(rosterPlayers);
                InitialRosterCollection = new ObservableCollection<MatchRoster>(initialRosterPlayers);

                PlayerCount = RosterCollection.Count;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(Fixture fixture, string teamID)
        {

            IsActivityIndicatorVisible = true;

            try
            {
                PlayerCount = 0;
                List<PlayerSeason> teamPlayers = await playerService.GetTeamPlayers(teamID);
                List<MatchRoster> matchRosterPlayers = await matchRosterService.GetTeamMatchRosters(fixture.ID, teamID);

                List<Player> playersList = teamPlayers.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
                
                matchRosterPlayers.ForEach(e => e.Player.IsStarter = e.IsStarter);
                List<Player> rosterPlayers = matchRosterPlayers.Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
                List<MatchRoster> initialRosterPlayers = matchRosterPlayers.OrderBy(e => e.IsStarter).ToList();

                var players = playersList.Where(p => !rosterPlayers.Any(p2 => p2.ID == p.ID));
                //get last teamsheet

                PlayerCollection = new ObservableCollection<Player>(players);
                RosterCollection = new ObservableCollection<Player>(rosterPlayers);
                InitialRosterCollection = new ObservableCollection<MatchRoster>(initialRosterPlayers);

                PlayerCount = RosterCollection.Count;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        private async void SelectedPlayerChanged(Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

            var items = e.AddedItems;

            if (items.Count == 0) return;


            var item = (Player)items[0];

            if (RosterCollection.Count < 18)
            {
                int starterCount = RosterCollection.Where(x => x.IsStarter).Count();

                if (RosterCollection.Count < 11 || starterCount < 11)
                {
                    item.IsStarter = true;
                }

                RosterCollection.Add(item);
                PlayerCollection.Remove(item);

                PlayerCount = RosterCollection.Count;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Line-Up", "Line up is full. You already have 18 players for the match.", "Okay");
            }
            //}
        }

        private void SelectedPlayerRemoved(Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {

            var items = e.AddedItems;

            if (items.Count == 0) return;


            var item = (Player)items[0];

            RosterCollection.Remove(item);
            PlayerCollection.Add(item);

            PlayerCount = RosterCollection.Count;

            //}
        }

        private void SelectedStarterChanged(object obj)
        {
            var player = obj as Player;

            
            RosterCollection.Remove(player);

            player.IsStarter = !player.IsStarter;
            RosterCollection.Add(player);

            //var item = (sender as ToggledEventArgs);
            //var model = ((e as SfSwitch).BindingContext as Player);
            //model.IsStarter = item.Value;

            //var items = e.NewValue;

            //if (items.Count == 0) return;


            //var item = (Player)items[0];

            //RosterCollection.Remove(item);
            //PlayerCollection.Add(item);

            //PlayerCount = RosterCollection.Count;

            //}
        }

        async Task Save()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                if(RosterCollection.Count >= 7)
                {
                    if(InitialRosterCollection.Count >= 7)
                    {
                        var rosters = RosterCollection.Except(InitialRosterCollection.Select(e => e.Player));

                        List<MatchRoster> matchRosterInsert = new List<MatchRoster>();
                        foreach (var player in rosters)
                        {
                            matchRosterInsert.Add(new MatchRoster
                            {
                                FixtureID = FixtureID,
                                TeamID = TeamID,
                                PlayerID = player.ID,
                                IsStarter = player.IsStarter,
                                JerseyNumber = player.JerseyNumber
                            });
                        }
                        await matchRosterService.InsertAll(matchRosterInsert);

                        bool update = false;
                        List<MatchRoster> matchRosterUpdate = new List<MatchRoster>();

                        foreach (var rosterPlayer in InitialRosterCollection)
                        {
                            foreach(var _player in RosterCollection)
                            {
                                if (rosterPlayer.PlayerID == _player.ID)
                                {
                                    if (rosterPlayer.IsStarter != _player.IsStarter)
                                    {
                                        rosterPlayer.IsStarter = _player.IsStarter;
                                        matchRosterUpdate.Add(rosterPlayer);
                                        update = true;
                                        break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (update)
                        {
                            await matchRosterService.UpdateAll(matchRosterUpdate);
                        }
                    }
                    else
                    {
                        List<MatchRoster> matchRoster = new List<MatchRoster>();
                        foreach (var player in RosterCollection)
                        {
                            matchRoster.Add(new MatchRoster
                            {
                                FixtureID = FixtureID,
                                TeamID = TeamID,
                                PlayerID = player.ID,
                                IsStarter = player.IsStarter,
                                JerseyNumber = player.JerseyNumber
                            });
                        }
                        await matchRosterService.InsertAll(matchRoster);
                    }

                    MessagingCenter.Send<string>("RosterList", "Refresh");
                    await Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Line-Up", "Please fill the line-up.", "Okay");
                }
            }
            catch (Exception ex)
            {
                //DisplayDataSavedErrorPromt();
                Debug.WriteLine(ex.Message, "MatchRoster");
            }
            finally
            {

                IsActivityIndicatorVisible = false;
            }
        }

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as Player;

            RosterCollection.Add(tappedInfo);
            PlayerCollection.Remove(tappedInfo);

            //if (tappedInfo.IsFavorite)
            //{
            //    secondLV.DataSource.Filter = FilterDepartures;
            //    tappedInfo.IsFavorite = false;
            //}
            //else
            //{
            //    secondLV.DataSource.Filter = null;
            //    tappedInfo.IsFavorite = true;
            //}
            //secondLV.DataSource.RefreshFilter();
        }
    }
}
