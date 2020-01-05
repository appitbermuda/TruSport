using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using TruSport.Services;
using System.Threading.Tasks;
using System.Diagnostics;
using TruSport.Views.Admin;

namespace TruSport.ViewModels
{
    public class UserPageViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedUserTypeChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onUserSelectedCommand;
        private TeamListView tappedInfo;
        private ObservableCollection<UserType> userTypeCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<string> userTypeNameCollection;
        private ObservableCollection<User> userCollection;
        private User userItem;
        private string firstName;
        private string lastName;
        private string email;
        private bool isValidated;
        private UserType userType;
        private Team team;
        private bool isTeamSelectable;
        private string setTitle;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;

        UserService userService;
        UserTypeService userTypeService;
        TeamService teamService;

        INavigation Navigation;

        #endregion

        #region Constructor

        public UserPageViewModel()
        {
            UserCollection = new ObservableCollection<User>();
            userService = new UserService();


            SetTitle = "Add";

            
            GenerateSource();

            UserSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(UserSelected);
        }

        public UserPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            UserCollection = new ObservableCollection<User>();
            userService = new UserService();


            SetTitle = "Add";


            GenerateSource();

            UserSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(UserSelected);
        }

        public UserPageViewModel(INavigation navigation, User user)
        {
            Navigation = navigation;
            userService = new UserService();
            userTypeService = new UserTypeService();
            teamService = new TeamService();

            SetTitle = "Edit";

            
            GenerateSource(user);

            SelectedUserTypeChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(UserTypeSelectionChanged);
            SelectedTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(TeamSelectionChanged);

            SaveCommand = new Command(async () => await Save());
        }

        //public PlayerDirectoryPageViewModel()
        //{
        //    PlayerDirectoryCollection = new ObservableCollection<PlayerListView>();
        //    Category = "U21";
        //    
        //    GenerateSource();
        //}

        #endregion

        #region Properties

        public Command SaveCommand { get; }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> UserSelectedCommand
        {
            get { return onUserSelectedCommand; }
            set { onUserSelectedCommand = value; }
        }

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
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedUserTypeChangedCommand
        {
            get { return selectedUserTypeChangedCommand; }
            set { selectedUserTypeChangedCommand = value; }
        }
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedTeamChangedCommand
        {
            get { return selectedTeamChangedCommand; }
            set { selectedTeamChangedCommand = value; }
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
        public ObservableCollection<User> UserCollection
        {
            get { return userCollection; }
            set { Set(ref this.userCollection, value); }
        }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref this.teamCollection, value); }
        }

        public User UserItem
        {
            get { return userItem; }
            set { Set(ref this.userItem, value); }
        }

        public string FirstName
        {
            get { return firstName; }
            set { Set(ref this.firstName, value); }
        }

        public string LastName
        {
            get { return lastName; }
            set { Set(ref this.lastName, value); }
        }

        public string Email
        {
            get { return firstName; }
            set { Set(ref this.firstName, value); }
        }

        public UserType UserType
        {
            get { return userType; }
            set { Set(ref this.userType, value); }
        }

        public Team Team
        {
            get { return team; }
            set { Set(ref this.team, value); }
        }

        public bool IsValidated
        {
            get { return isValidated; }
            set { Set(ref this.isValidated, value); }
        }

        public bool IsTeamSelectable
        {
            get { return isTeamSelectable; }
            set { Set(ref this.isTeamSelectable, value); }
        }

        public string SetTitle
        {
            get { return setTitle; }
            set { this.setTitle = value; }
        }

        public ObservableCollection<UserType> UserTypeCollection
        {
            get { return userTypeCollection; }
            set { Set(ref this.userTypeCollection, value); }
        }

        public ObservableCollection<string> UserTypeNameCollection
        {
            get { return userTypeNameCollection; }
            set { this.userTypeNameCollection = value; }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(User user)
        {
            IsActivityIndicatorVisible = true;

            UserItem = user;

            try
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
                UserType = user.UserType;
                Team = user.Team;
                IsValidated = user.IsValidated;

                var userTypes = await userTypeService.GetUserTypes();
                UserTypeCollection = new ObservableCollection<UserType>(userTypes);

                if (UserItem.UserType.Name.Contains("Team"))
                {
                    IsTeamSelectable = true;
                    var teams = await teamService.GetTeams();

                    teams.ForEach(e => e.Team.League = e.League);
                    teams.ForEach(e => e.Team.LeagueID = e.LeagueID);

                    TeamCollection = new ObservableCollection<Team>(teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).OrderBy(e => e.League.Name));
                }
                else
                    IsTeamSelectable = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                var users = await userService.GetAll();

                if(users != null)
                    UserCollection = new ObservableCollection<User>(users);
                else
                    await Application.Current.MainPage.DisplayAlert("Unauthorized", "You are not authorized to view this page.", "Okay");

                IsActivityIndicatorVisible = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "User Admin");
            }
        }

        private async void UserTypeSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _userType = e.Value as UserType;

            if (_userType.Name.Contains("Team"))
            {
                IsTeamSelectable = true;
                var teams = await teamService.GetTeams();
                TeamCollection = new ObservableCollection<Team>(teams.Where(x => x.Season.IsCurrent).Select(x => x.Team));
            }
            else
                IsTeamSelectable = false;

            UserType = _userType;
            
            UserItem.UserType = _userType;
            UserItem.UserTypeID = _userType.ID;
        }

        private void TeamSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _team = e.Value as Team;

            
            Team = _team;
            
            UserItem.Team = _team;
            UserItem.TeamID = _team.ID;
        }

        async Task Save()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                //UserItem.FirstName = FirstName;
                //UserItem.LastName = LastName;
                //UserItem.Email = Email;
                //UserItem.IsValidated = IsValidated;

                await userService.Update(UserItem);

                MessagingCenter.Send<string>("UserAdminList", "Refresh");
                await Navigation.PopAsync();

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving team, please try again.", "Okay");
            }
        }

        //private void ResetTapped(object obj)
        //{
        //    secondLV.DataSource.Filter = null;
        //    secondLV.DataSource.RefreshFilter();
        //    firstLV.AllowSwiping = true;
        //}

        //private void FavoriteTapped(object obj)
        //{
        //    var departureInfo = obj as DepartureInfo;
        //    var pinnedInfo = FirstLVCollection.Any(o => o.Name == departureInfo.Name) ? FirstLVCollection.First(o => o.Name == departureInfo.Name) : null;
        //    if (pinnedInfo == null)
        //    {
        //        FirstLVCollection.Add(new PinnedInfo() { Name = departureInfo.Name, RouteName = departureInfo.Name, Icon = departureInfo.Icon, IsFavorite = true });
        //    }
        //}

        private async void UserSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as User;

            if (item != null)
            {
                //CancelFixtureRefresh = false;
                MessagingCenter.Subscribe<string>("UserAdminList", "Refresh", async (sender) =>
                {
                    MessagingCenter.Unsubscribe<string>("UserAdminList", "Refresh");

                    IsActivityIndicatorVisible = true;

                    var users = await userService.GetAll();

                    if (users != null)
                        UserCollection = new ObservableCollection<User>(users);
                    else
                        await Application.Current.MainPage.DisplayAlert("Unauthorized", "You are not authorized to view this page.", "Okay");

                    IsActivityIndicatorVisible = false;
                });

                await Navigation.PushAsync(new EditUserPage(item));
            }
        }

        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as TeamListView;
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

        //private bool FilterDepartures(object obj)
        //{
        //    var departureInfo = obj as DepartureInfo;
        //    if (tappedInfo == null)
        //        return true;

        //    if (departureInfo.Name.ToLower().Contains(tappedInfo.Name.ToLower())
        //         || departureInfo.RouteName.ToLower().Contains(tappedInfo.RouteName.ToLower()))
        //        return true;
        //    else
        //        return false;
        //}

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
