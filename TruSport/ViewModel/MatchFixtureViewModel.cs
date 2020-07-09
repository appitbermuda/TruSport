using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views.MatchConfigurations;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MatchFixtureViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchTypeChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedHomeTeamChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedAwayTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onHomeTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onAwayTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onMatchSummarySelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Fixture fixtureItem;
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
        MatchStatService matchStatService;
        MatchService matchService;

        INavigation Navigation;

        #endregion

        public MatchFixtureViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FixturesCollection = new ObservableCollection<Fixture>();
            fixtureService = new FixtureService();

            GenerateSource();

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
        }

        public MatchFixtureViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            fixtureService = new FixtureService();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            matchService = new MatchService();

            FixtureItem = fixture;

            GenerateSource(fixture);

            BackCommand = new Command(async () => await Back());
            EditCommand = new Command(async () => await Edit());
            OnGoalClickedCommand = new Command(async () => await OnGoalClicked());
            EditHomeTeamCommand = new Command(async () => await OnEditHomeTeam());
            EditAwayTeamCommand = new Command(async () => await OnEditAwayTeam());
            SelectHomeTeamCommand = new Command(async () => await OnSelectHomeTeam());
            SelectAwayTeamCommand = new Command(async () => await OnSelectAwayTeam());
            OnHomeTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(HomeTeamSelect);
            OnAwayTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(AwayTeamSelect);
            OnMatchSummarySelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(MatchSummarySelected);

        }

        public Command SelectHomeTeamCommand { get; }
        public Command SelectAwayTeamCommand { get; }
        public Command OnGoalClickedCommand { get; }
        public Command EditHomeTeamCommand { get; }
        public Command EditAwayTeamCommand { get; }
        public Command BackCommand { get; }
        public Command EditCommand { get; }

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

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnMatchSummarySelectedCommand
        {
            get { return onMatchSummarySelectedCommand; }
            set { onMatchSummarySelectedCommand = value; }
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

        public bool IsPenalties
        {
            get { return _isPenalties; }
            set { Set(ref _isPenalties, value); }
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

            List<Fixture> fixtures = await fixtureService.GetFixtures();
            FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.Date <= DateTime.Now.Date));

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            //IsPenalties = false;

            //fixture = await fixtureService.Get(fixture.ID);

            var matchRostersList = await matchRosterService.GetFixtureMatchRosters(fixture.ID);

            FixtureItem = fixture;

            //IsPenalties = fixture.Match.IsPenalties ?? false;

            if (matchRostersList == null)
            {
                HomeTeamSelected = false;
                AwayTeamSelected = false;
            }
            else
            {
                var homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID);
                var awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID);

                if (homeRoster.Count() == 0)
                    HomeTeamSelected = false;
                else
                {
                    HomeTeamSelected = true;

                    HomeRosterCollection = new ObservableCollection<MatchRoster>(homeRoster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                }

                if (awayRoster.Count() == 0)
                    AwayTeamSelected = false;
                else
                {
                    AwayTeamSelected = true;

                    AwayRosterCollection = new ObservableCollection<MatchRoster>(awayRoster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                }

                List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();
                bool IsHomeTeam = false;

                //var matchRosters = homeRoster.Union(awayRoster).ToList();
                //foreach (var roster in matchRosters)
                //{
                //    if (roster.TeamID == fixture.HomeTeamID)
                //        IsHomeTeam = true;
                //    else
                //        IsHomeTeam = false;

                //    if (roster.MatchStats.Count > 0)
                //    {
                //        for (var i = 0; i < roster.MatchStats.Count; i++)
                //        {
                //            if (roster.MatchStats[i].Goal > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchRosterSummary
                //                {
                //                    MatchStatID = roster.MatchStats[i].ID,
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                //                    AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
                //                    Minute = roster.MatchStats[i].GoalTime ?? 0,
                //                    Goal = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }

                //            if (roster.MatchStats[i].YellowCard > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchRosterSummary
                //                {
                //                    MatchStatID = roster.MatchStats[i].ID,
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                //                    YellowCard = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }

                //            if (roster.MatchStats[i].RedCard > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchRosterSummary
                //                {
                //                    MatchStatID = roster.MatchStats[i].ID,
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    Minute = roster.MatchStats[i].RedCardTime ?? 0,
                //                    RedCard = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }
                //        }
                //    }

                //    if (roster.SubstitutePlayerID != null && roster.IsStarter)
                //    {
                //        matchRosterSummaries.Add(new MatchRosterSummary
                //        {
                //            FixtureID = roster.FixtureID,
                //            TeamID = roster.TeamID,
                //            PlayerID = roster.PlayerID,
                //            PlayerName = roster.Player.Name,
                //            SubstitutePlayerID = roster.SubstitutePlayerID,
                //            SubstitutePlayerName = roster.SubstitutePlayer.Name,
                //            Minute = roster.SubstituteTime ?? 0,
                //            IsSub = true,
                //            IsHomeTeam = IsHomeTeam
                //        });

                //    }
                //}

                if (matchRosterSummaries.Any(e => e.Minute == -1))
                    MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries);
                else
                    MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

            }

            IsActivityIndicatorVisible = false;

        }

        internal async Task RefreshMatchSummary(Fixture fixture)
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
            var homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID);
            var awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID);

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
            bool IsHomeTeam = false;

            //var matchRosters = homeRoster.Union(awayRoster).ToList();
            //foreach (var roster in matchRosters)
            //{
            //    if (roster.TeamID == fixture.HomeTeamID)
            //        IsHomeTeam = true;
            //    else
            //        IsHomeTeam = false;

            //    if (roster.MatchStats.Count > 0)
            //    {
            //        for (var i = 0; i < roster.MatchStats.Count; i++)
            //        {
            //            if (roster.MatchStats[i].Goal > 0)
            //            {
            //                matchRosterSummaries.Add(new MatchRosterSummary
            //                {
            //                    MatchStatID = roster.MatchStats[i].ID,
            //                    FixtureID = roster.FixtureID,
            //                    TeamID = roster.TeamID,
            //                    PlayerID = roster.PlayerID,
            //                    PlayerName = roster.Player.Name,
            //                    AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
            //                    AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
            //                    Minute = roster.MatchStats[i].GoalTime ?? 0,
            //                    Goal = 1,
            //                    IsHomeTeam = IsHomeTeam
            //                });
            //            }

            //            if (roster.MatchStats[i].YellowCard > 0)
            //            {
            //                matchRosterSummaries.Add(new MatchRosterSummary
            //                {
            //                    MatchStatID = roster.MatchStats[i].ID,
            //                    FixtureID = roster.FixtureID,
            //                    TeamID = roster.TeamID,
            //                    PlayerID = roster.PlayerID,
            //                    PlayerName = roster.Player.Name,
            //                    Minute = roster.MatchStats[i].YellowCardTime ?? 0,
            //                    YellowCard = 1,
            //                    IsHomeTeam = IsHomeTeam
            //                });
            //            }

            //            if (roster.MatchStats[i].RedCard > 0)
            //            {
            //                matchRosterSummaries.Add(new MatchRosterSummary
            //                {
            //                    MatchStatID = roster.MatchStats[i].ID,
            //                    FixtureID = roster.FixtureID,
            //                    TeamID = roster.TeamID,
            //                    PlayerID = roster.PlayerID,
            //                    PlayerName = roster.Player.Name,
            //                    Minute = roster.MatchStats[i].RedCardTime ?? 0,
            //                    RedCard = 1,
            //                    IsHomeTeam = IsHomeTeam
            //                });
            //            }
            //        }
            //    }

            //    if (roster.SubstitutePlayerID != null && roster.IsStarter)
            //    {
            //        matchRosterSummaries.Add(new MatchRosterSummary
            //        {
            //            FixtureID = roster.FixtureID,
            //            TeamID = roster.TeamID,
            //            PlayerID = roster.PlayerID,
            //            PlayerName = roster.Player.Name,
            //            SubstitutePlayerID = roster.SubstitutePlayerID,
            //            SubstitutePlayerName = roster.SubstitutePlayer.Name,
            //            Minute = roster.SubstituteTime ?? 0,
            //            IsSub = true,
            //            IsHomeTeam = IsHomeTeam
            //        });

            //    }
            //}

            if (matchRosterSummaries.Any(e => e.Minute == -1))
                MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries);
            else
                MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

            //}

            IsActivityIndicatorVisible = false;

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

            await Navigation.PushAsync(new EditFixturePage(FixtureItem));
        }

        async Task Back()
        {

            MessagingCenter.Send<string>("FixtureList", "Refresh");
            await Navigation.PopAsync();



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
                    var roster = matchRostersList.Where(e => e.TeamID == FixtureItem.HomeTeamID);

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
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    AwayTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == FixtureItem.AwayTeamID);

                    if (roster.Count() == 0)
                        AwayTeamSelected = false;
                    else
                    {
                        AwayTeamSelected = true;

                        AwayRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem.ID, FixtureItem.AwayTeamID, FixtureItem.AwayTeam.Name));
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
                    var roster = matchRostersList.Where(e => e.TeamID == FixtureItem.HomeTeamID);

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
            MessagingCenter.Subscribe<string>("RosterList", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("RosterList", "Refresh");

                IsActivityIndicatorVisible = true;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(FixtureItem.ID);
                if (matchRostersList == null)
                {
                    AwayTeamSelected = false;
                }
                else
                {
                    var roster = matchRostersList.Where(e => e.TeamID == FixtureItem.AwayTeamID);

                    if (roster.Count() == 0)
                        AwayTeamSelected = false;
                    else
                    {
                        AwayTeamSelected = true;

                        AwayRosterCollection = new ObservableCollection<MatchRoster>(roster.OrderByDescending(e => e.IsStarter).ThenBy(e => e.Player.LastName));
                    }
                }
                IsActivityIndicatorVisible = false;
            });

            await Navigation.PushAsync(new EditMatchRosterPage(FixtureItem, FixtureItem.AwayTeamID));
        }

        private async void HomeTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;

            //if (items.Count == 0) return;

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;


            MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
                await RefreshMatchSummary(FixtureItem);
            });

            await Navigation.PushModalAsync(new MatchPopupPage(item, FixtureItem));
        }

        private async void AwayTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRoster;

            //if (items.Count == 0) return;

            MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
                await RefreshMatchSummary(FixtureItem);
            });

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

            await Navigation.PushModalAsync(new MatchPopupPage(item, FixtureItem));
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
                    List<Fixture> fixtures = await fixtureService.GetFixtures();
                    FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(x => x.Date <= DateTime.Now.Date));
                    IsActivityIndicatorVisible = false;
                });

                //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

                await Navigation.PushAsync(new FixturePage(item));
            }
            //FixtureList.SelectedItems.Clear();
        }

        private async void MatchSummarySelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as MatchRosterSummary;

            MessagingCenter.Unsubscribe<string>("MatchSummary", "Refresh");

            MessagingCenter.Subscribe<string>("MatchSummary", "Refresh", async (sender) =>
            {
                IsActivityIndicatorVisible = true;

                await RefreshMatchSummary(FixtureItem);

                IsActivityIndicatorVisible = false;
            });

            var delete = await Application.Current.MainPage.DisplayAlert("Delete Stat","Do you want to delete this?","Delete","Cancel");

            if(delete)
            {
                try
                {

                    //await matchStatService.Remove(item.MatchStatID);

                    if(item.Goal > 0)
                    {
                        if (item.TeamID == FixtureItem.HomeTeamID)
                            FixtureItem.Match.HomeTeamScore = FixtureItem.Match.HomeTeamScore - 1;
                        else
                            FixtureItem.Match.AwayTeamScore = FixtureItem.Match.AwayTeamScore - 1;

                        await matchService.Update(FixtureItem.Match);

                        MatchRosterSummaryCollection.Remove(item);
                    }
                    
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
