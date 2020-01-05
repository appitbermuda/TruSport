using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.MatchConfigurations;
using TruSport.Views.MyTeam;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class TeamRosterViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchTypeChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedHomeTeamChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedAwayTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onHomeTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onAwayTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onPlayerSelectedCommand;
        private ObservableCollection<PlayerSeason> playerCollection;
        private Player playerItem;
        private PlayerSeason playerSeasonItem;
        private string teamName;
        private bool _isActivityIndicatorVisible;
        PlayerService playerService;
        INavigation Navigation;

        #endregion

        public TeamRosterViewModel(INavigation navigation)
        {
            Navigation = navigation;
            PlayerCollection = new ObservableCollection<PlayerSeason>();
            playerService = new PlayerService();

            

            GenerateSource();

            OnPlayerSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(PlayerSelected);
        }

        public TeamRosterViewModel(INavigation navigation, string playerID)
        {
            Navigation = navigation;
            PlayerCollection = new ObservableCollection<PlayerSeason>();
            playerService = new PlayerService();

            GenerateSource(playerID);

            SaveCommand = new Command(async () => await Save());
            OnPlayerSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(PlayerSelected);
        }

        public Command SelectHomeTeamCommand { get; }
        public Command SelectAwayTeamCommand { get; }
        public Command EditHomeTeamCommand { get; }
        public Command EditAwayTeamCommand { get; }
        public Command BackCommand { get; }
        public Command EditCommand { get; }
        public Command SaveCommand { get; }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnPlayerSelectedCommand
        {
            get { return onPlayerSelectedCommand; }
            set { onPlayerSelectedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnHomeTeamSelectedCommand
        {
            get { return onHomeTeamSelectedCommand; }
            set { onHomeTeamSelectedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnAwayTeamSelectedCommand
        {
            get { return onAwayTeamSelectedCommand; }
            set { onAwayTeamSelectedCommand = value; }
        }

        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedFieldChangedCommand
        {
            get { return selectedFieldChangedCommand; }
            set { selectedFieldChangedCommand = value; }
        }
        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedLeagueChangedCommand
        {
            get { return selectedLeagueChangedCommand; }
            set { selectedLeagueChangedCommand = value; }
        }
        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedMatchTypeChangedCommand
        {
            get { return selectedMatchTypeChangedCommand; }
            set { selectedMatchTypeChangedCommand = value; }
        }
        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedHomeTeamChangedCommand
        {
            get { return selectedHomeTeamChangedCommand; }
            set { selectedHomeTeamChangedCommand = value; }
        }
        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedAwayTeamChangedCommand
        {
            get { return selectedAwayTeamChangedCommand; }
            set { selectedAwayTeamChangedCommand = value; }
        }

        public ObservableCollection<PlayerSeason> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public Player PlayerItem
        {
            get { return playerItem; }
            set { Set(ref playerItem, value); }
        }

        public PlayerSeason PlayerSeasonItem
        {
            get { return playerSeasonItem; }
            set { Set(ref playerSeasonItem, value); }
        }

        public string TeamName
        {
            get { return teamName; }
            set { Set(ref teamName, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {

            IsActivityIndicatorVisible = true;

            var teamID = await SecureStorage.GetAsync("TeamID");

            if (teamID == null)
            {
                SecureStorage.RemoveAll();
                App.Current.MainPage = new FootballMasterDetailPage();
            }
            else
            {
                
                var players = await playerService.GetTeamPlayers(teamID);

                TeamName = players.FirstOrDefault().Team.Name;

                PlayerCollection = new ObservableCollection<PlayerSeason>(players.Where(e => e.Season.IsCurrent).OrderBy(e => e.Player.LastName));
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(string playerID)
        {

            IsActivityIndicatorVisible = true;

            //var teamID = await SecureStorage.GetAsync("TeamID");

            //if (teamID == null)
            //{
            //    SecureStorage.RemoveAll();
            //    App.Current.MainPage = new FootballMasterDetailPage();
            //}
            //else
            //{

            var player = await playerService.Get(playerID);

            //player.Player.Played = player.GamesPlayed ?? 0;
            //player.Player.Goals = player.Goals ?? 0;
            //player.Player.YellowCards = player.YellowCards ?? 0;
            //player.Player.RedCards = player.RedCards ?? 0;

            PlayerSeasonItem = player;
            //PlayerItem = player.Player;

                //TeamName = players.FirstOrDefault().Team.Name;

                //players.ForEach(e => e.Player.Goals = e.Goals ?? 0);
                //players.ForEach(e => e.Player.RedCards = e.RedCards ?? 0);
                //players.ForEach(e => e.Player.YellowCards = e.YellowCards ?? 0);



                //PlayerCollection = new ObservableCollection<Player>(players.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName));
            //}

            IsActivityIndicatorVisible = false;

        }

        async Task Save()
        {

            try
            {
                IsActivityIndicatorVisible = true;

                
                if (await playerService.Update(PlayerSeasonItem))
                {
                    //await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                    MessagingCenter.Send<string>("PlayerList", "Refresh");
                    await Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Issue saving player, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving player", "Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private async void PlayerSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
    {
            var item = e.ItemData as PlayerSeason;

            //if (items.Count == 0) return;
            if (item != null)
            {

                MessagingCenter.Subscribe<string>("PlayerList", "Refresh", async (sender) =>
                {
                    MessagingCenter.Unsubscribe<string>("PlayerList", "Refresh");

                    IsActivityIndicatorVisible = true;

                    var teamID = await SecureStorage.GetAsync("TeamID");

                    if (teamID == null)
                    {
                        SecureStorage.RemoveAll();
                        App.Current.MainPage = new FootballMasterDetailPage();
                    }
                    else
                    {

                        var players = await playerService.GetTeamPlayers(teamID);

                        PlayerCollection = new ObservableCollection<PlayerSeason>(players.OrderBy(x => x.Player.LastName));
                    }

                    IsActivityIndicatorVisible = false;
                });

                //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

                await Navigation.PushAsync(new EditPlayerPage(item.Player.ID));
            }
            //FixtureList.SelectedItems.Clear();
        }
    }
}
