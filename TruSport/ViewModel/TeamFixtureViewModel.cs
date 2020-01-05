using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
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
    public class TeamFixtureViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchTypeChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedHomeTeamChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedAwayTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onHomeTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onAwayTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Fixture fixtureItem;
        private string teamName;
        private string teamID;
        private DateTime matchDate;
        private TimeSpan matchTime;
        private string setTitle;
        private string fieldName;
        private string leagueName;
        private string matchTypeName;
        private string homeTeamName;
        private string homeTeamID;
        private string awayTeamName;
        private string awayTeamID;
        private int homeTeamScore;
        private int homeTeamPenalty;
        private int awayTeamScore;
        private int awayTeamPenalty;
        private bool isPenalties;
        private bool isPostponed;
        private ObservableCollection<string> leagueCollection;
        private ObservableCollection<string> matchTypeCollection;
        private ObservableCollection<string> fieldCollection;
        private ObservableCollection<string> teamCollection;
        private ObservableCollection<Fixture> fixturesCollection;
        private ObservableCollection<MatchRoster> homeRosterCollection;
        private ObservableCollection<MatchRoster> awayRosterCollection;
        private ObservableCollection<MatchRosterSummary> matchRosterSummaryCollection;
        private bool _isPenalties;
        private bool _homeTeamSelected;
        private bool _squadsSelected;
        private bool _awayTeamSelected;
        private bool _isActivityIndicatorVisible;
        FixtureService fixtureService;
        MatchRosterService matchRosterService;
        INavigation Navigation;

        #endregion

        public TeamFixtureViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FixturesCollection = new ObservableCollection<Fixture>();
            fixtureService = new FixtureService();

            GenerateSource();

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
        }


        public TeamFixtureViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            fixtureService = new FixtureService();
            matchRosterService = new MatchRosterService();

            FixtureItem = fixture;

            GenerateSource(fixture);

            BackCommand = new Command(async () => await Back());
            EditCommand = new Command(async () => await Edit());
            OnGoalClickedCommand = new Command(async () => await OnGoalClicked());
            EditHomeTeamCommand = new Command(async () => await OnEditTeam());
            EditAwayTeamCommand = new Command(async () => await OnEditAwayTeam());
            SelectHomeTeamCommand = new Command(async () => await OnSelectTeam());
            SelectAwayTeamCommand = new Command(async () => await OnSelectAwayTeam());
            OnHomeTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(HomeTeamSelect);
            OnAwayTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(AwayTeamSelect);

        }

        public TeamFixtureViewModel(INavigation navigation, Fixture fixture, string edit)
        {
            Navigation = navigation;
            fixtureService = new FixtureService();
            matchRosterService = new MatchRosterService();

            FixtureItem = fixture;

            GenerateEditSource(fixture);

            BackCommand = new Command(async () => await Back());
            SaveCommand = new Command(async () => await Save());
        }

        public Command OnGoalClickedCommand { get; }
        public Command SelectHomeTeamCommand { get; }
        public Command SelectAwayTeamCommand { get; }
        public Command EditHomeTeamCommand { get; }
        public Command EditAwayTeamCommand { get; }
        public Command BackCommand { get; }
        public Command EditCommand { get; }
        public Command SaveCommand { get; }

        public Fixture FixtureItem
        {
            get { return fixtureItem; }
            set { Set(ref fixtureItem, value); }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
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

        public ObservableCollection<string> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
        }

        public ObservableCollection<string> FieldCollection
        {
            get { return fieldCollection; }
            set { Set(ref fieldCollection, value); }
        }

        public ObservableCollection<string> MatchTypeCollection
        {
            get { return matchTypeCollection; }
            set { Set(ref matchTypeCollection, value); }
        }

        public ObservableCollection<string> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public ObservableCollection<Fixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<MatchRoster> HomeRosterCollection
        {
            get { return homeRosterCollection; }
            set { Set(ref homeRosterCollection, value); }
        }

        public ObservableCollection<MatchRoster> AwayRosterCollection
        {
            get { return awayRosterCollection; }
            set { Set(ref awayRosterCollection, value); }
        }

        public ObservableCollection<MatchRosterSummary> MatchRosterSummaryCollection
        {
            get { return matchRosterSummaryCollection; }
            set { Set(ref matchRosterSummaryCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public int HomeTeamScore
        {
            get { return homeTeamScore; }
            set { Set(ref this.homeTeamScore, value); }
        }

        public int HomeTeamPenalty
        {
            get { return homeTeamPenalty; }
            set { Set(ref this.homeTeamPenalty, value); }
        }

        public int AwayTeamScore
        {
            get { return awayTeamScore; }
            set { Set(ref this.awayTeamScore, value); }
        }

        public int AwayTeamPenalty
        {
            get { return awayTeamPenalty; }
            set { Set(ref this.awayTeamPenalty, value); }
        }

        public DateTime MatchDate
        {
            get { return matchDate; }
            set { Set(ref this.matchDate, value); }
        }

        public TimeSpan MatchTime
        {
            get { return matchTime; }
            set { Set(ref this.matchTime, value); }
        }

        public string LeagueName
        {
            get { return leagueName; }
            set { Set(ref this.leagueName, value); }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { Set(ref this.fieldName, value); }
        }

        public string MatchTypeName
        {
            get { return matchTypeName; }
            set { Set(ref this.matchTypeName, value); }
        }

        public string TeamName
        {
            get { return teamName; }
            set { Set(ref this.teamName, value); }
        }

        public string TeamID
        {
            get { return teamID; }
            set { Set(ref this.teamID, value); }
        }

        public string HomeTeamName
        {
            get { return homeTeamName; }
            set { Set(ref this.homeTeamName, value); }
        }

        public string HomeTeamID
        {
            get { return homeTeamID; }
            set { Set(ref this.homeTeamID, value); }
        }

        public string AwayTeamName
        {
            get { return awayTeamName; }
            set { Set(ref this.awayTeamName, value); }
        }

        public string AwayTeamID
        {
            get { return awayTeamID; }
            set { Set(ref this.awayTeamID, value); }
        }

        public bool IsPostponed
        {
            get { return isPostponed; }
            set { Set(ref isPostponed, value); }
        }

        public bool IsPenalties
        {
            get { return isPostponed; }
            set { Set(ref isPostponed, value); }
        }

        public bool AwayTeamSelected
        {
            get { return _awayTeamSelected; }
            set { Set(ref _awayTeamSelected, value); }
        }

        public bool HomeTeamSelected
        {
            get { return _homeTeamSelected; }
            set { Set(ref _homeTeamSelected, value); }
        }

        internal async void GenerateSource()
        {

            IsActivityIndicatorVisible = true;

            TeamID = await SecureStorage.GetAsync("TeamID");

            if (TeamID == null)
            {
                SecureStorage.RemoveAll();
                App.Current.MainPage = new FootballMasterDetailPage();
            }
            else
            {
                List<Fixture> fixtures = await fixtureService.GetTeamFixtures(TeamID);

                var fixture = fixtures.FirstOrDefault();
                TeamName = fixture.AwayTeamID == TeamID ? fixture.AwayTeam.Name : fixture.HomeTeam.Name;
                FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.Date <= DateTime.Now.Date));
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateEditSource(Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            try
            {
                fixture = await fixtureService.Get(fixture.ID);
                FixtureItem = fixture;
                HomeTeamName = fixture.HomeTeam.Name;
                AwayTeamName = fixture.AwayTeam.Name;
                HomeTeamScore = fixture.Match.HomeTeamScore ?? 0;
                AwayTeamScore = fixture.Match.AwayTeamScore ?? 0;
                IsPostponed = fixture.IsPostponed;
                IsPenalties = fixture.Match.IsPenalties ?? false;
                HomeTeamPenalty = fixture.Match.HomeTeamPenalty ?? 0;
                AwayTeamPenalty = fixture.Match.AwayTeamPenalty ?? 0;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            TeamID = await SecureStorage.GetAsync("TeamID");

            if (TeamID == null)
            {
                SecureStorage.RemoveAll();
                App.Current.MainPage = new FootballMasterDetailPage();
            }
            else
            {
                TeamName = fixture.AwayTeamID == TeamID ? fixture.AwayTeam.Name : fixture.HomeTeam.Name;

                List<Fixture> fixtures = await fixtureService.GetTeamFixtures(TeamID);
                FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.Date <= DateTime.Now.Date));

                //IsPenalties = false;

                //fixture = await fixtureService.Get(fixture.ID);

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(fixture.ID);

                FixtureItem = fixture;

                

                //IsPenalties = fixture.Match.IsPenalties ?? false;

                if (matchRostersList == null)
                {
                    HomeTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == TeamID);

                    if (roster.Count() == 0)
                        HomeTeamSelected = false;
                    else
                    {
                        HomeTeamSelected = true;

                        HomeRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }

                    List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();

                    var matchRosters = roster.ToList();
                    foreach (var player in matchRosters)
                    {
                        if (player.MatchStats.Count > 0)
                        {
                            for (var i = 0; i < player.MatchStats.Count; i++)
                            {
                                if (player.MatchStats[i].Goal > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = player.FixtureID,
                                        TeamID = player.TeamID,
                                        PlayerID = player.PlayerID,
                                        PlayerName = player.Player.Name,
                                        AssistPlayerID = player.MatchStats[i].AssistPlayerID,
                                        AssistPlayerName = player.MatchStats[i].AssistPlayer != null ? player.MatchStats[i].AssistPlayer.Name : null,
                                        Minute = player.MatchStats[i].GoalTime ?? 0,
                                        Goal = 1
                                    });
                                }

                                if (player.MatchStats[i].YellowCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = player.FixtureID,
                                        TeamID = player.TeamID,
                                        PlayerID = player.PlayerID,
                                        PlayerName = player.Player.Name,
                                        Minute = player.MatchStats[i].YellowCardTime ?? 0,
                                        YellowCard = 1
                                    });
                                }

                                if (player.MatchStats[i].RedCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = player.FixtureID,
                                        TeamID = player.TeamID,
                                        PlayerID = player.PlayerID,
                                        PlayerName = player.Player.Name,
                                        Minute = player.MatchStats[i].RedCardTime ?? 0,
                                        RedCard = 1
                                    });
                                }
                            }
                        }

                        if (player.SubstitutePlayerID != null && player.IsStarter)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = player.FixtureID,
                                TeamID = player.TeamID,
                                PlayerID = player.PlayerID,
                                PlayerName = player.Player.Name,
                                SubstitutePlayerID = player.SubstitutePlayerID,
                                SubstitutePlayerName = player.SubstitutePlayer.Name,
                                Minute = player.SubstituteTime ?? 0,
                                IsSub = true
                            });

                        }
                    }

                    MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

                }

            }

            IsActivityIndicatorVisible = false;

        }

        internal async void RefreshMatchList()
        {
            IsActivityIndicatorVisible = true;

            TeamID = await SecureStorage.GetAsync("TeamID");

            if (TeamID == null)
            {
                SecureStorage.RemoveAll();
                App.Current.MainPage = new FootballMasterDetailPage();
            }
            else
            {
                List<Fixture> fixtures = await fixtureService.GetTeamFixtures(TeamID);

                var fixture = fixtures.FirstOrDefault();
                TeamName = fixture.AwayTeamID == TeamID ? fixture.AwayTeam.Name : fixture.HomeTeam.Name;
                FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.Date <= DateTime.Now.Date));
            }

            IsActivityIndicatorVisible = false;
        }

            internal async void RefreshMatchSummary(Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            fixture = await fixtureService.Get(fixture.ID);
            FixtureItem = fixture;

            var matchRostersList = await matchRosterService.GetFixtureMatchRosters(fixture.ID);

            //if (fixture.MatchRosters == null)
            //{
            //    HomeTeamSelected = false;
            //    AwayTeamSelected = false;
            //}
            //else
            //{
            var homeRoster = matchRostersList.Where(e => e.TeamID == TeamID);

            //if (homeRoster.Count() == 0)
            //    HomeTeamSelected = false;
            //else
            //{
            //    HomeTeamSelected = true;

            //    HomeRosterCollection = new ObservableCollection<MatchRoster>(homeRoster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
            //}

            //if (awayRoster.Count() == 0)
            //    AwayTeamSelected = false;
            //else
            //{
            //    AwayTeamSelected = true;

            //    AwayRosterCollection = new ObservableCollection<MatchRoster>(awayRoster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
            //}

            List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();
            

            var matchRosters = homeRoster.ToList();
            foreach (var roster in matchRosters)
            {
                if (roster.MatchStats.Count > 0)
                {
                    for (var i = 0; i < roster.MatchStats.Count; i++)
                    {
                        if (roster.MatchStats[i].Goal > 0)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                PlayerName = roster.Player.Name,
                                AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                                AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
                                Minute = roster.MatchStats[i].GoalTime ?? 0,
                                Goal = 1
                            });
                        }

                        if (roster.MatchStats[i].YellowCard > 0)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                PlayerName = roster.Player.Name,
                                Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                                YellowCard = 1
                            });
                        }

                        if (roster.MatchStats[i].RedCard > 0)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                PlayerName = roster.Player.Name,
                                Minute = roster.MatchStats[i].RedCardTime ?? 0,
                                RedCard = 1
                            });
                        }
                    }
                }

                if (roster.SubstitutePlayerID != null && roster.IsStarter)
                {
                    matchRosterSummaries.Add(new MatchRosterSummary
                    {
                        FixtureID = roster.FixtureID,
                        TeamID = roster.TeamID,
                        PlayerID = roster.PlayerID,
                        PlayerName = roster.Player.Name,
                        SubstitutePlayerID = roster.SubstitutePlayerID,
                        SubstitutePlayerName = roster.SubstitutePlayer.Name,
                        Minute = roster.SubstituteTime ?? 0,
                        IsSub = true
                    });

                }
            }

            MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

            //}

            IsActivityIndicatorVisible = false;

        }

        async Task OnGoalClicked()
        {

            MessagingCenter.Unsubscribe<Fixture>(this, "Update");
            MessagingCenter.Subscribe<Fixture>(this, "Update", async (fixture) =>
            {

                IsActivityIndicatorVisible = true;

                FixtureItem = new Fixture();
                FixtureItem = fixture;

                IsActivityIndicatorVisible = false;

            });

            await Navigation.PushModalAsync(new MatchGoalPopupPage(FixtureItem));
        }

        async Task Edit()
        {
            

            MessagingCenter.Unsubscribe<Fixture>(this, "Update");
            MessagingCenter.Subscribe<Fixture>(this, "Update", async (fixture) =>
            {
                
                IsActivityIndicatorVisible = true;
                FixtureItem = fixture;
                
                IsActivityIndicatorVisible = false;
                
            });

            await Navigation.PushAsync(new UpdateMatchStatsPage(FixtureItem));
        }

        async Task Back()
        {

            MessagingCenter.Send<string>("FixtureList", "Refresh");
            await Navigation.PopAsync();
            

            
        }

        async Task OnSelectTeam()
        {
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    HomeTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == TeamID);

                    if (roster.Count() == 0)
                        HomeTeamSelected = false;
                    else
                    {
                        HomeTeamSelected = true;

                        HomeRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem.ID, TeamID, TeamName));
        }

        async Task OnSelectHomeTeam()
        {
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    HomeTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == TeamID);

                    if (roster.Count() == 0)
                        HomeTeamSelected = false;
                    else
                    {
                        HomeTeamSelected = true;

                        HomeRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                    IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem.ID, FixtureItem.HomeTeamID, FixtureItem.HomeTeam.Name));
        }

        async Task OnSelectAwayTeam()
        {
            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem.ID, FixtureItem.AwayTeamID, FixtureItem.AwayTeam.Name));
        }

        async Task OnEditTeam()
        {
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    HomeTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == TeamID);

                    if (roster.Count() == 0)
                        HomeTeamSelected = false;
                    else
                    {
                        HomeTeamSelected = true;

                        HomeRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem, FixtureItem.HomeTeamID));
        }

        async Task OnEditHomeTeam()
        {
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    HomeTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == TeamID);

                    if (roster.Count() == 0)
                        HomeTeamSelected = false;
                    else
                    {
                        HomeTeamSelected = true;

                        HomeRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem, FixtureItem.HomeTeamID));
        }

        async Task OnEditAwayTeam()
        {
            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem, FixtureItem.AwayTeamID));
        }

        private async void HomeTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;

            //if (items.Count == 0) return;

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;


            MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
                RefreshMatchSummary(FixtureItem);
            });

            await Navigation.PushModalAsync(new MatchPopupPage(item));
        }

        private async void AwayTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;

            //if (items.Count == 0) return;

            MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
                RefreshMatchSummary(FixtureItem);
            });

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

            await Navigation.PushModalAsync(new MatchPopupPage(item));
        }

    private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
    {
            var item = e.ItemData as Fixture;

            //if (items.Count == 0) return;
            if (item != null)
            {

                MessagingCenter.Subscribe<string>("FixtureList", "Refresh", async (sender) =>
                {
                    MessagingCenter.Unsubscribe<string>("FixtureList", "Refresh");

                    IsActivityIndicatorVisible = true;
                    var teamID = await SecureStorage.GetAsync("TeamID");

                    List<Fixture> fixtures = await fixtureService.GetTeamFixtures(teamID);
                    FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(x => x.Date <= DateTime.Now.Date));
                    IsActivityIndicatorVisible = false;
                });

                //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

                await Navigation.PushAsync(new TeamFixturePage(item));
            }
            //FixtureList.SelectedItems.Clear();
        }

        async Task Save()
        {

            try
            {
                var save = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to save this fixture? Please make sure everything is correct.", "Yes", "Cancel");

                if (save)
                {
                    IsActivityIndicatorVisible = true;

                    FixtureItem.Match.HomeTeamScore = HomeTeamScore;
                    FixtureItem.Match.AwayTeamScore = AwayTeamScore;
                    FixtureItem.IsPostponed = IsPostponed;
                    FixtureItem.Match.IsPenalties = IsPenalties;
                    FixtureItem.Match.HomeTeamPenalty = HomeTeamPenalty;
                    FixtureItem.Match.AwayTeamPenalty = AwayTeamPenalty;

                    if (await fixtureService.Update(FixtureItem))
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                        MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture", "Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }
    }

    
}
