using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class AssignMatchViewModel : BaseViewModel
    {

        private Player tappedInfo;
        private Fixture _fixtureItem;
        private MatchRoster _rosterPlayer;
        private MatchRoster _alternateRosterPlayer;
        private ObservableCollection<Player> homeTeamCollection;
        private ObservableCollection<Player> awayTeamCollection;
        private Command<object> onSaveGoalCommand;
        private Command<object> onSaveSubCommand;
        //private Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs> selectedStarterChanged;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> selectedAssistCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> selectedSubstituteCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onHomeTeamSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onAwayTeamSelectedCommand;
        private ObservableCollection<Player> playerCollection;
        private ObservableCollection<Player> rosterCollection;
        private ObservableCollection<Player> assistCollection;
        private ObservableCollection<Player> subCollection;
        private string assistPlayerID;
        private string substitutePlayerID;
        private string _teamName;
        private int homeTeamScore;
        private int awayTeamScore;
        private int playerCount;
        private int minute;
        private bool goalSelected;
        private bool subSelected;
        private bool redCardSelected;
        private bool yellowCardSelected;
        private bool isOwnGoal;
        private bool isAssistVisible;
        private bool isSubstituteVisible;

        PlayerService playerService;
        MatchRosterService matchRosterService;
        MatchService matchService;
        MatchStatService matchStatService;
        INavigation Navigation;

        private bool _isActivityIndicatorVisible;

        public AssignMatchViewModel(INavigation navigation)
        {
            Navigation = navigation;

            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            matchRosterService = new MatchRosterService();
            matchService = new MatchService();
            matchStatService = new MatchStatService();
            playerService = new PlayerService();

            GenerateSource();
        }

        public AssignMatchViewModel(INavigation navigation, MatchRoster rosterPlayer)
        {
            Navigation = navigation;
            RosterPlayer = rosterPlayer;
            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            matchService = new MatchService();
            playerService = new PlayerService();

            GenerateSource(rosterPlayer);

            OnSaveGoalCommand = new Command(async () => await SaveGoal());
            OnSaveSubCommand = new Command(async () => await SaveSub());

            OnGoalSelected = new Command(async () => await GoalSelected());
            OnYellowCardSelected = new Command(async () => await YellowCardSelected());
            OnRedCardSelected = new Command(async () => await RedCardSelected());
            OnSubSelected = new Command(async () => await SubSelected());

            //SelectedStarterCommand = new Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs>(SelectedStarterChanged);
            SelectedAssistCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedAssistChanged);
            SelectedSubstituteCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedSubstituteChanged);
        }

        public AssignMatchViewModel(INavigation navigation, MatchRoster rosterPlayer, Fixture fixture)
        {
            Navigation = navigation;
            RosterPlayer = rosterPlayer;
            PlayerCollection = new ObservableCollection<Player>();
            RosterCollection = new ObservableCollection<Player>();
            matchRosterService = new MatchRosterService();
            matchStatService = new MatchStatService();
            matchService = new MatchService();
            playerService = new PlayerService();

            GenerateSource(rosterPlayer, fixture);

            OnSaveGoalCommand = new Command(async () => await SaveGoal());
            OnSaveSubCommand = new Command(async () => await SaveSub());

            OnGoalSelected = new Command(async () => await GoalSelected());
            OnYellowCardSelected = new Command(async () => await YellowCardSelected());
            OnRedCardSelected = new Command(async () => await RedCardSelected());
            OnSubSelected = new Command(async () => await SubSelected());

            //SelectedStarterCommand = new Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs>(SelectedStarterChanged);
            SelectedAssistCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedAssistChanged);
            SelectedSubstituteCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedSubstituteChanged);
        }

        public AssignMatchViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            HomeTeamCollection = new ObservableCollection<Player>();
            AwayTeamCollection = new ObservableCollection<Player>();

            matchRosterService = new MatchRosterService();
            matchService = new MatchService();
            playerService = new PlayerService();

            GenerateSource(fixture);


            HomeTeamSubtractClickedCommand = new Command(async () => await HomeTeamSubtract());
            HomeTeamAddClickedCommand = new Command(async () => await HomeTeamAdd());
            AwayTeamSubtractClickedCommand = new Command(async () => await AwayTeamSubtract());
            AwayTeamAddClickedCommand = new Command(async () => await AwayTeamAdd());

            OnMatchGoalSavedCommand = new Command(async () => await MatchGoalSaved());
            OnHomeTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(HomeTeamSelect);
            OnAwayTeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(AwayTeamSelect);
        }

        public Command OnGoalSelected { get; }
        public Command OnYellowCardSelected { get; }
        public Command OnRedCardSelected { get; }
        public Command OnSubSelected { get; }

        public Command OnMatchGoalSavedCommand { get; }
        public Command OnSaveGoalCommand { get; }
        public Command OnSaveSubCommand { get; }

        public Command HomeTeamSubtractClickedCommand { get; }
        public Command HomeTeamAddClickedCommand { get; }
        public Command AwayTeamSubtractClickedCommand { get; }
        public Command AwayTeamAddClickedCommand { get; }


        //public Command<Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs> SelectedStarterCommand
        //{
        //    get { return selectedStarterChanged; }
        //    set { selectedStarterChanged = value; }
        //}

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> SelectedAssistCommand
        {
            get { return selectedAssistCommand; }
            set { selectedAssistCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> SelectedSubstituteCommand
        {
            get { return selectedSubstituteCommand; }
            set { selectedSubstituteCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
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

        public ObservableCollection<Player> HomeTeamCollection
        {
            get { return homeTeamCollection; }
            set { Set(ref homeTeamCollection, value); }
        }

        public ObservableCollection<Player> AwayTeamCollection
        {
            get { return awayTeamCollection; }
            set { Set(ref awayTeamCollection, value); }
        }

        public ObservableCollection<Player> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public ObservableCollection<Player> AssistCollection
        {
            get { return assistCollection; }
            set { Set(ref assistCollection, value); }
        }

        public ObservableCollection<Player> SubCollection
        {
            get { return subCollection; }
            set { Set(ref subCollection, value); }
        }

        public ObservableCollection<Player> RosterCollection
        {
            get { return rosterCollection; }
            set { Set(ref rosterCollection, value); }
        }


        public int HomeTeamScore
        {
            get { return homeTeamScore; }
            set { Set(ref homeTeamScore, value); }
        }

        public int AwayTeamScore
        {
            get { return awayTeamScore; }
            set { Set(ref awayTeamScore, value); }
        }

        public string AssistPlayerID
        {
            get { return assistPlayerID; }
            set { Set(ref assistPlayerID, value); }
        }

        public string SubstitutePlayerID
        {
            get { return substitutePlayerID; }
            set { Set(ref substitutePlayerID, value); }
        }

        public string TeamName
        {
            get { return _teamName; }
            set { Set(ref _teamName, value); }
        }

        public Fixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public MatchRoster RosterPlayer
        {
            get { return _rosterPlayer; }
            set { Set(ref _rosterPlayer, value); }
        }

        public MatchRoster AlternateRosterPlayer
        {
            get { return _alternateRosterPlayer; }
            set { Set(ref _alternateRosterPlayer, value); }
        }

        public int Minute
        {
            get { return minute; }
            set { Set(ref minute, value); }
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

        public bool GoalButtonSelected
        {
            get { return goalSelected; }
            set { Set(ref goalSelected, value); }
        }

        public bool SubButtonSelected
        {
            get { return subSelected; }
            set { Set(ref subSelected, value); }
        }

        public bool RedCardButtonSelected
        {
            get { return redCardSelected; }
            set { Set(ref redCardSelected, value); }
        }

        public bool YellowCardButtonSelected
        {
            get { return yellowCardSelected; }
            set { Set(ref yellowCardSelected, value); }
        }

        public bool IsOwnGoal
        {
            get { return isOwnGoal; }
            set { Set(ref isOwnGoal, value); }
        }

        public bool IsAssistVisible
        {
            get { return isAssistVisible; }
            set { Set(ref isAssistVisible, value); }
        }

        public bool IsSubstituteVisible
        {
            get { return isSubstituteVisible; }
            set { Set(ref isSubstituteVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                Minute = 1;
                List<PlayerSeason> players = await playerService.GetPlayers();
                PlayerCollection = new ObservableCollection<Player>(players.Where(e => e.Season.IsCurrent).Select(e => e.Player));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(MatchRoster rosterPlayer)
        {

            IsActivityIndicatorVisible = true;

            try
            {

                Minute = 1;
                RedCardButtonSelected = false;
                YellowCardButtonSelected = false;
                GoalButtonSelected = false;
                SubButtonSelected = false;
                IsSubstituteVisible = false;
                IsAssistVisible = false;


                //List<PlayerSeason> players = await playerService.GetTeamPlayers(teamID);

                //PlayerCollection = new ObservableCollection<Player>(players.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(MatchRoster rosterPlayer, Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            try
            {
                var gameTime = fixture.Date.Add(TimeSpan.Parse(fixture.Time));

                if (gameTime.AddMinutes(115) < DateTime.Now && gameTime > DateTime.Now)
                    Minute = 1;
                else
                {
                    var gameMinutes = Convert.ToInt32(DateTime.Now.Subtract(gameTime).TotalMinutes);

                    if (gameMinutes > 60)
                        Minute = gameMinutes - 15;
                    else
                        Minute = gameMinutes;
                }

                RedCardButtonSelected = false;
                YellowCardButtonSelected = false;
                GoalButtonSelected = false;
                SubButtonSelected = false;
                IsSubstituteVisible = false;
                IsAssistVisible = false;


                //List<PlayerSeason> players = await playerService.GetTeamPlayers(teamID);

                //PlayerCollection = new ObservableCollection<Player>(players.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
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

            try
            {
                List<PlayerSeason> homeTeamPlayers = await playerService.GetTeamPlayers(fixture.HomeTeamID);
                List<PlayerSeason> awayTeamPlayers = await playerService.GetTeamPlayers(fixture.AwayTeamID);

                HomeTeamCollection = new ObservableCollection<Player>(homeTeamPlayers.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList());
                AwayTeamCollection = new ObservableCollection<Player>(awayTeamPlayers.Where(e => e.Season.IsCurrent).Select(e => e.Player).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList());

                HomeTeamScore = fixture.Match.HomeTeamScore ?? 0;
                AwayTeamScore = fixture.Match.AwayTeamScore ?? 0;

                FixtureItem = fixture;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }


        async Task GoalSelected()
        {
            try
            {
                RedCardButtonSelected = false;
                YellowCardButtonSelected = false;
                GoalButtonSelected = true;
                SubButtonSelected = false;


                var roster = await matchRosterService.GetTeamMatchRosters(RosterPlayer.FixtureID, RosterPlayer.TeamID);

                roster = roster.Where(e => e.PlayerID != RosterPlayer.PlayerID && ((e.IsStarter && e.SubstitutePlayerID == null) || (!e.IsStarter && e.SubstitutePlayerID != null)) && e.MatchStats.All(x => ((x.RedCard.HasValue && x.RedCard == 0) || x.RedCard == null) || ((x.YellowCard.HasValue && x.YellowCard == 0) || x.YellowCard == null))).ToList();

                AssistCollection = new ObservableCollection<Player>(roster.Select(e => e.Player));

                IsAssistVisible = true;
                IsSubstituteVisible = false;

            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        private async Task SaveGoal()
        {
            try
            {
                var match = await matchService.GetFixtureMatch(RosterPlayer.FixtureID);

                if (IsOwnGoal)
                {
                    if (match.Fixture.HomeTeamID == RosterPlayer.TeamID)
                    {
                        match.AwayTeamScore = match.AwayTeamScore == null ? 1 : match.AwayTeamScore + 1;
                    }
                    else
                    {
                        match.HomeTeamScore = match.HomeTeamScore == null ? 1 : match.HomeTeamScore + 1;
                    }
                }
                else
                {
                    if (match.Fixture.HomeTeamID == RosterPlayer.TeamID)
                    {
                        match.HomeTeamScore = match.HomeTeamScore == null ? 1 : match.HomeTeamScore + 1;
                    }
                    else
                    {
                        match.AwayTeamScore = match.AwayTeamScore == null ? 1 : match.AwayTeamScore + 1;
                    }
                }

                //RosterPlayer.Goals = RosterPlayer.Goals == null ? 1 : RosterPlayer.Goals + 1;



                //await matchService.Update(match);
                //await matchRosterService.Update(RosterPlayer);
                

                //if (AssistPlayerID != null)
                //{
                //    RosterPlayer.AssistPlayerID = AssistPlayerID;


                //    var assistPlayer = await matchRosterService.GetMatchRosterPlayer(RosterPlayer.FixtureID, AssistPlayerID);

                //    assistPlayer.Assists = assistPlayer.Assists == null ? 1 : assistPlayer.Assists + 1;

                //    //await matchRosterService.Update(assistPlayer);
                //}



            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        private async Task MatchGoalSaved()
        {
            try
            {
                var match = await matchService.GetFixtureMatch(FixtureItem.ID);

                if (match.HomeTeamScore != HomeTeamScore || match.AwayTeamScore != AwayTeamScore)
                {

                    match.HomeTeamScore = HomeTeamScore;
                    match.AwayTeamScore = AwayTeamScore;

                    await matchService.Update(match);

                    FixtureItem.Match = match;
                }

                MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        async Task YellowCardSelected()
        {
            try
            {
                RedCardButtonSelected = false;
                YellowCardButtonSelected = true;
                GoalButtonSelected = false;
                SubButtonSelected = false;

                IsSubstituteVisible = false;
                IsAssistVisible = false;

                MatchStat insertMatchStat = new MatchStat
                {
                    YellowCard = 1,
                    MatchRosterID = RosterPlayer.ID,
                    YellowCardTime = Minute,
                };

                var saveCard = await App.Current.MainPage.DisplayAlert("Yellow Card", "Give yellow card to " + RosterPlayer.Player.Name + "?", "Yes", "Cancel");
                if (saveCard)
                {
                    await matchStatService.Insert(insertMatchStat);

                    await Navigation.PopModalAsync();

                    MessagingCenter.Send<String>("AssignRoster", "Refresh");
                }



            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                YellowCardButtonSelected = false;
                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        async Task RedCardSelected()
        {
            try
            {
                RedCardButtonSelected = true;
                YellowCardButtonSelected = false;
                GoalButtonSelected = false;
                SubButtonSelected = false;

                IsSubstituteVisible = false;
                IsAssistVisible = false;



                MatchStat insertMatchStat = new MatchStat
                {
                    RedCard = 1,
                    MatchRosterID = RosterPlayer.ID,
                    RedCardTime = Minute,
                };


                var saveCard = await App.Current.MainPage.DisplayAlert("Red Card", "Give red card to " + RosterPlayer.Player.Name + "?", "Yes", "Cancel");
                if (saveCard)
                {
                    await matchStatService.Insert(insertMatchStat);

                    await Navigation.PopModalAsync();

                    MessagingCenter.Send<String>("AssignRoster", "Refresh");
                }

            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                RedCardButtonSelected = false;
                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        async Task SubSelected()
        {
            try
            {
                RedCardButtonSelected = false;
                YellowCardButtonSelected = false;
                GoalButtonSelected = false;
                SubButtonSelected = true;

                var roster = await matchRosterService.GetTeamMatchRosters(RosterPlayer.FixtureID, RosterPlayer.TeamID);
                var subs = roster.Where(e => !e.IsStarter);
                List<MatchRoster> cleanSubs = new List<MatchRoster>();

                foreach (var sub in subs)
                {
                    int count = 0;
                    foreach (var player in roster)
                    {
                        if (sub.PlayerID == player.SubstitutePlayerID)
                        {
                            count++;
                            break;
                        }
                    }

                    if (count == 0)
                        cleanSubs.Add(sub);
                }

                SubCollection = new ObservableCollection<Player>(cleanSubs.Select(e => e.Player));

                IsSubstituteVisible = true;
                IsAssistVisible = false;
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        private async Task SaveSub()
        {
            try
            {
                if (SubstitutePlayerID != null)
                {
                    RosterPlayer.SubstitutePlayerID = SubstitutePlayerID;
                    RosterPlayer.SubstituteTime = Minute;

                    var subPlayer = await matchRosterService.GetMatchRosterPlayer(RosterPlayer.FixtureID, SubstitutePlayerID);

                    subPlayer.SubstitutePlayerID = RosterPlayer.PlayerID;
                    subPlayer.SubstituteTime = Minute;

                    await matchRosterService.Update(RosterPlayer);
                    await matchRosterService.Update(subPlayer);
                }
            }
            catch (Exception ex)
            {

                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        private async void SelectedAssistChanged(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                Player assistPlayer = new Player();
                if (e != null)
                {
                    var item = e.ItemData as Player;

                    //if (items.Count == 0) return;

                    //var item = (Player)items[0];
                    assistPlayer = item;
                    AssistPlayerID = item.ID;
                }

                var match = await matchService.GetFixtureMatch(RosterPlayer.FixtureID);

                var matchRoster = await matchRosterService.GetFixtureMatchRosters(RosterPlayer.FixtureID);
                //var matchStats = matchRoster.Select(x => x.MatchStats);

                //var stats = matchStats.Where(x => x.Any(i => i.Goal > 0)).ToList();

                int homeGoalCount = 0;
                int awayGoalCount = 0;

                match.HomeTeamScore = match.HomeTeamScore ?? 0;
                match.AwayTeamScore = match.AwayTeamScore ?? 0;

                foreach (var player in matchRoster)
                {
                    foreach(var stat in player.MatchStats)
                    {
                        if (player.TeamID == RosterPlayer.TeamID && ((match.Fixture.HomeTeamID == RosterPlayer.TeamID && !IsOwnGoal) || (match.Fixture.AwayTeamID == RosterPlayer.TeamID && IsOwnGoal)) && stat.Goal > 0)
                        {
                            homeGoalCount = homeGoalCount + 1;
                        }
                        else if (player.TeamID == RosterPlayer.TeamID && ((match.Fixture.AwayTeamID == RosterPlayer.TeamID && !IsOwnGoal) || (match.Fixture.HomeTeamID == RosterPlayer.TeamID && IsOwnGoal)) && stat.Goal > 0)
                        {
                            awayGoalCount = awayGoalCount + 1;
                        }
                    }
                    
                }

                if ((match.Fixture.HomeTeamID == RosterPlayer.TeamID && IsOwnGoal) || (match.Fixture.AwayTeamID == RosterPlayer.TeamID && !IsOwnGoal))
                {
                    if(awayGoalCount >= match.AwayTeamScore)
                        match.AwayTeamScore = awayGoalCount + 1;
                }
                else if ((match.Fixture.AwayTeamID == RosterPlayer.TeamID && IsOwnGoal) || (match.Fixture.HomeTeamID == RosterPlayer.TeamID && !IsOwnGoal))
                {
                    if(homeGoalCount >= match.HomeTeamScore)
                    match.HomeTeamScore = homeGoalCount + 1;
                }

                MatchStat insertMatchStat = new MatchStat
                {
                    Goal = 1,
                    MatchRosterID = RosterPlayer.ID,
                    GoalTime = Minute,
                };


                if (AssistPlayerID != null)
                {

                    insertMatchStat.AssistPlayerID = AssistPlayerID;

                    var saveCard = await App.Current.MainPage.DisplayAlert("Goal", "Scorer: " + RosterPlayer.Player.Name + "\n\nAssist: " + assistPlayer.Name, "Yes", "Cancel");
                    if (saveCard)
                    {

                        if ((((match.Fixture.HomeTeamID == RosterPlayer.TeamID && !IsOwnGoal) || (match.Fixture.AwayTeamID == RosterPlayer.TeamID && IsOwnGoal)) && homeGoalCount >= match.HomeTeamScore - 1)
                            || (((match.Fixture.AwayTeamID == RosterPlayer.TeamID && !IsOwnGoal) || (match.Fixture.HomeTeamID == RosterPlayer.TeamID && IsOwnGoal)) && awayGoalCount >= match.AwayTeamScore - 1))
                        {
                            await matchService.Update(match);
                        }


                        ////await matchRosterService.Update(RosterPlayer);
                        await matchStatService.Update(insertMatchStat);

                        await Task.Delay(300);
                        await Navigation.PopModalAsync();

                        MessagingCenter.Send<String>("AssignRoster", "Refresh");
                    }
                }
                else
                {
                    var saveCard = await App.Current.MainPage.DisplayAlert("Goal!", "Scorer: " + RosterPlayer.Player.Name, "Yes", "Cancel");
                    if (saveCard)
                    {
                        if (homeGoalCount == match.HomeTeamScore - 1 || awayGoalCount == match.AwayTeamScore - 1)
                        {
                            await matchService.Update(match);
                        }
                        await matchStatService.Update(insertMatchStat);

                        await Navigation.PopModalAsync();

                        MessagingCenter.Send<String>("AssignRoster", "Refresh");
                    }
                }


            }
            catch (Exception ex)
            {
                //await Navigation.PopModalAsync();
                //await App.Current.MainPage.DisplayAlert("Error", "There was an issue saving your request. Please try again.", "Okay");
            }

            IsAssistVisible = false;
            GoalButtonSelected = false;
        }

        private async void SelectedSubstituteChanged(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                var item = e.ItemData as Player;

                //if (items.Count == 0) return;

                //var item = (Player)items[0];

                SubstitutePlayerID = item.ID;

                if (SubstitutePlayerID != null)
                {
                    RosterPlayer.SubstitutePlayerID = SubstitutePlayerID;
                    RosterPlayer.SubstituteTime = Minute;

                    var subPlayer = await matchRosterService.GetMatchRosterPlayer(RosterPlayer.FixtureID, SubstitutePlayerID);

                    subPlayer.SubstitutePlayerID = RosterPlayer.PlayerID;
                    subPlayer.SubstituteTime = Minute;

                    var saveCard = await App.Current.MainPage.DisplayAlert("Substitute", "OUT: " + RosterPlayer.Player.Name + "\n\nIN: " + subPlayer.Player.Name, "Yes", "Cancel");
                    if (saveCard)
                    {
                        await matchRosterService.Update(RosterPlayer);
                        await matchRosterService.Update(subPlayer);

                        await Navigation.PopModalAsync();

                        MessagingCenter.Send<String>("AssignRoster", "Refresh");
                    }
                }



            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "There was an issue saving your request. Please try again.", "Okay");
            }

            IsSubstituteVisible = false;
            SubButtonSelected = false;
        }

        private async void HomeTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Player;

            try
            {
                var addGoal = await Application.Current.MainPage.DisplayAlert("Goal Scored", "Add a goal score by " + item.FirstName + " " + item.LastName + "?", "Okay", "Cancel");

                if (addGoal)
                {
                    var playerInRoster = await matchRosterService.GetMatchRosterPlayer(FixtureItem.ID, item.ID);

                    if (playerInRoster != null)
                    {
                        MatchStat newMatchStat = new MatchStat
                        {
                            Goal = 1,
                            GoalTime = -1,
                            MatchRosterID = playerInRoster.ID
                        };



                        //if (addGoal)
                        //{
                            var updated = await matchStatService.Update(newMatchStat);

                            if (updated)
                                HomeTeamScore = HomeTeamScore + 1;
                        //}
                    }
                    else
                    {
                        MatchStat newMatchStat = new MatchStat
                        {
                            Goal = 1,
                            GoalTime = -1
                        };

                        List<MatchStat> matchStats = new List<MatchStat>();
                        matchStats.Add(newMatchStat);

                        MatchRoster matchRoster = new MatchRoster
                        {
                            PlayerID = item.ID,
                            TeamID = FixtureItem.HomeTeamID,
                            FixtureID = FixtureItem.ID,
                            MatchStats = matchStats
                        };

                        //var addGoal = await Application.Current.MainPage.DisplayAlert("Goal Scored", "Add a goal score by " + item.FirstName + " " + item.LastName + "?", "Okay", "Cancel");

                        //if (addGoal)
                        //{
                            var inserted = await matchRosterService.Insert(matchRoster);

                            if (inserted)
                                HomeTeamScore = HomeTeamScore + 1;

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Update Score", "Looks like there was an issue updating the scorer", "Okay");
            }

            //if (items.Count == 0) return;

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;


            //MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", (sender) =>
            //{
            //    MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
            //    RefreshMatchSummary(FixtureItem);
            //});

            //await Navigation.PushModalAsync(new MatchPopupPage(item, FixtureItem));
        }

        private async void AwayTeamSelect(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Player;

            try
            {
                var addGoal = await Application.Current.MainPage.DisplayAlert("Goal Scored", "Add a goal score by " + item.FirstName + " " + item.LastName + "?", "Okay", "Cancel");

                if (addGoal)
                {
                    var playerInRoster = await matchRosterService.GetMatchRosterPlayer(FixtureItem.ID, item.ID);

                    if (playerInRoster != null)
                    {
                        MatchStat newMatchStat = new MatchStat
                        {
                            Goal = 1,
                            GoalTime = -1,
                            MatchRosterID = playerInRoster.ID
                        };



                        //if (addGoal)
                        //{
                        var updated = await matchStatService.Update(newMatchStat);

                        if (updated)
                            AwayTeamScore = AwayTeamScore + 1;
                        //}
                    }
                    else
                    {
                        MatchStat newMatchStat = new MatchStat
                        {
                            Goal = 1,
                            GoalTime = -1
                        };

                        List<MatchStat> matchStats = new List<MatchStat>();
                        matchStats.Add(newMatchStat);

                        MatchRoster matchRoster = new MatchRoster
                        {
                            PlayerID = item.ID,
                            TeamID = FixtureItem.AwayTeamID,
                            FixtureID = FixtureItem.ID,
                            MatchStats = matchStats
                        };

                        //var addGoal = await Application.Current.MainPage.DisplayAlert("Goal Scored", "Add a goal score by " + item.FirstName + " " + item.LastName + "?", "Okay", "Cancel");

                        //if (addGoal)
                        //{
                        var inserted = await matchRosterService.Insert(matchRoster);

                        if (inserted)
                            AwayTeamScore = AwayTeamScore + 1;

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Update Score", "Looks like there was an issue updating the scorer", "Okay");
            }
            //if (items.Count == 0) return;

            //MessagingCenter.Subscribe<string>("AssignRoster", "Refresh", (sender) =>
            //{
            //    MessagingCenter.Unsubscribe<string>("AssignRoster", "Refresh");
            //    RefreshMatchSummary(FixtureItem);
            //});

            //var item = (MatchRoster)HomeRosterItemListView.SelectedItem;

            //await Navigation.PushModalAsync(new MatchPopupPage(item, FixtureItem));
        }

        private async Task HomeTeamAdd()
        {
            try
            {
                HomeTeamScore = HomeTeamScore + 1;
            }
            catch (Exception ex)
            {

            }
        }

        private async Task HomeTeamSubtract()
        {
            try
            {
                HomeTeamScore = HomeTeamScore - 1;
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AwayTeamAdd()
        {
            try
            {
                AwayTeamScore = AwayTeamScore + 1;
            }
            catch (Exception ex)
            {

            }
        }

        private async Task AwayTeamSubtract()
        {
            try
            {
                AwayTeamScore = AwayTeamScore - 1;
            }
            catch (Exception ex)
            {

            }
        }

        //private async void SelectedPlayerChanged(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        //{

        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;


        //    var item = (Player)items[0];

        //    if (RosterCollection.Count < 18)
        //    {
        //        if(RosterCollection.Count < 11)
        //        {
        //            item.IsStarter = true;
        //        }

        //        RosterCollection.Add(item);
        //        PlayerCollection.Remove(item);

        //        PlayerCount = RosterCollection.Count;
        //    }
        //    else
        //    {
        //        await App.Current.MainPage.DisplayAlert("Line-Up", "Line up is full. You already have 18 players for the match.", "Okay");
        //    }
        //    //}
        //}

        //private void SelectedPlayerRemoved(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        //{

        //    var items = e.AddedItems;

        //    if (items.Count == 0) return;


        //    var item = (Player)items[0];

        //    RosterCollection.Remove(item);
        //    PlayerCollection.Add(item);

        //    PlayerCount = RosterCollection.Count;

        //    //}
        //}

        //private void SelectedStarterChanged(object obj)
        //{
        //    var player = obj as Player;


        //    RosterCollection.Remove(player);

        //    player.IsStarter = !player.IsStarter;
        //    RosterCollection.Add(player);

        //    //var item = (sender as ToggledEventArgs);
        //    //var model = ((e as SfSwitch).BindingContext as Player);
        //    //model.IsStarter = item.Value;

        //    //var items = e.NewValue;

        //    //if (items.Count == 0) return;


        //    //var item = (Player)items[0];

        //    //RosterCollection.Remove(item);
        //    //PlayerCollection.Add(item);

        //    //PlayerCount = RosterCollection.Count;

        //    //}
        //}

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
