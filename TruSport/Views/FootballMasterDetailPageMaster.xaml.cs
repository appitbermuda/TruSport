using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TruSport.Data;
using TruSport.Views.Admin;
using TruSport.Views.Football;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMasterDetailPageMaster : ContentPage
    {
        public SfListView OnTrackListView;
        public SfListView AdminListView;
        public SfListView MatchListView;
        public SfListView TeamListView;


        public FootballMasterDetailPageMaster()
        {


            InitializeComponent();

            BindingContext = new FootballMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
            MatchListView = MatchMenuItemsListView;
            TeamListView = TeamMenuItemsListView;
            AdminListView = AdminMenuItemsListView;
        }

        async void SignOutTapped(object sender, System.EventArgs e)
        {
            try
            {
                var signout = await DisplayAlert("Sign Out","Are your sure you want to sign out?","Sign Out","Cancel");

                if (signout)
                {
                    SignOutLabel.BackgroundColor = (Color)App.Current.Resources["primaryBarBlue"];

                    SecureStorage.RemoveAll();

                    App.IsLoggedIn = false;

                    App.Current.MainPage = new FootballMasterDetailPage();
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("SIGN OUT", "There was an issue logging you out. Please try again.", "Okay");
            }
            finally
            {
                SignOutLabel.BackgroundColor = Color.Transparent;
            }
        }

        class FootballMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public bool SignOutVisible { get; set; }
            public bool TeamAdminVisible { get; set; }
            public bool AdminVisible { get; set; }
            public bool MatchCommissionerVisible { get; set; }
            public double OnTrackHeight { get; set; }
            public double MatchConfigHeight { get; set; }
            public double TeamHeight { get; set; }
            public double AdminHeight { get; set; }
            AuthorizationHandler authorizationHandler;

            public ObservableCollection<FootballMasterDetailPageMenuItem> AdminMenuItems { get; set; }
            public ObservableCollection<FootballMasterDetailPageMenuItem> OnTrackMenuItems { get; set; }
            public ObservableCollection<FootballMasterDetailPageMenuItem> MatchMenuItems { get; set; }
            public ObservableCollection<FootballMasterDetailPageMenuItem> TeamMenuItems { get; set; }

            public FootballMasterDetailPageMasterViewModel()
            {
                SignOutVisible = false;

                authorizationHandler = new AuthorizationHandler();

                GenerateMenu();
            }

            internal async void GenerateMenu()
            {
                var userLoggedIn = await SecureStorage.GetAsync("UserLoggedIn");
                var token = await SecureStorage.GetAsync("Token");

                if (await authorizationHandler.UserInRole(Constants.MatchCommissioner) && userLoggedIn != null & userLoggedIn == "true" && token != null)
                {
                    SignOutVisible = true;
                    MatchCommissionerVisible = true;
                    TeamAdminVisible = false;
                    AdminVisible = false;

                    OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(TablePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(LeagueStatsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(CompetitionsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(FavouritePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                        //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" }
                    });

                    OnTrackHeight = 50 * OnTrackMenuItems.Count;

                    MatchMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Match's", IconSource="" , TargetType = typeof(MatchConfigurations.FixtureListPage), Group = "Match Configurations" }
                    });

                    MatchConfigHeight = 50 * MatchMenuItems.Count;

                }
                else if (await authorizationHandler.UserInRole(Constants.TeamAdministrator) && userLoggedIn != null & userLoggedIn == "true" && token != null)
                {
                    SignOutVisible = true;
                    TeamAdminVisible = true;
                    AdminVisible = false;
                    MatchCommissionerVisible = false;

                    OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(TablePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(LeagueStatsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(CompetitionsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(FavouritePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                        //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" }
                    });

                    OnTrackHeight = 50 * OnTrackMenuItems.Count;

                    TeamMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Squad", IconSource="" , TargetType = typeof(MyTeam.RosterPage), Group = "My Team" },
                        //Change to authenticate for team from sign in
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Match's", IconSource="" , TargetType = typeof(MyTeam.MatchListPage), Group = "My Team" }
                    });

                    TeamHeight = 50 * TeamMenuItems.Count;
                }
                else if (await authorizationHandler.UserInRole(Constants.Administrator) && userLoggedIn != null & userLoggedIn == "true" && token != null)
                {
                    SignOutVisible = true;
                    MatchCommissionerVisible = true;
                    TeamAdminVisible = false;
                    AdminVisible = true;

                    OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(TablePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(LeagueStatsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(CompetitionsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(FavouritePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                        //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" }
                    });

                    OnTrackHeight = 50 * OnTrackMenuItems.Count;

                    MatchMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Match's", IconSource="" , TargetType = typeof(MatchConfigurations.FixtureListPage), Group = "Match Configurations" }
                    });

                    MatchConfigHeight = 50 * MatchMenuItems.Count;

                    AdminMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Teams", IconSource="" , TargetType = typeof(Admin.TeamPage), Group = "Admin" },
                        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Players", IconSource="" , TargetType = typeof(Admin.PlayerPage), Group = "Admin" },
                        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Fixtures", IconSource="" , TargetType = typeof(Admin.FixturePage), Group = "Admin" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Users", IconSource="" , TargetType = typeof(Admin.UserPage), Group = "Admin" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Notifications", IconSource="" , TargetType = typeof(Admin.PushNotifications), Group = "Admin" }
                    });
                    //
                    //AdminMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    //{
                    //    new FootballMasterDetailPageMenuItem { Id = 0, Title = "Coaches", IconSource="" , TargetType = typeof(Admin.CoachPage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 1, Title = "Field", IconSource="" , TargetType = typeof(Admin.FieldPage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 2, Title = "Fixtures", IconSource="" , TargetType = typeof(Admin.FixturePage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leagues", IconSource="" , TargetType = typeof(Admin.LeaguePage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 4, Title = "Players", IconSource="" , TargetType = typeof(Admin.PlayerPage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 5, Title = "Teams", IconSource="" , TargetType = typeof(Admin.TeamPage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 6, Title = "Users", IconSource="" , TargetType = typeof(Admin.UserPage), Group = "Admin" },
                    //    new FootballMasterDetailPageMenuItem { Id = 7, Title = "User Types", IconSource="" , TargetType = typeof(Admin.UserTypeEditPage), Group = "Admin" }
                    //});

                    AdminHeight = 50 * AdminMenuItems.Count;
                }
                else
                {
                    MatchCommissionerVisible = false;
                    TeamAdminVisible = false;
                    AdminVisible = false;
                    
                    OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                    {
                        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(TablePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(LeagueStatsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(CompetitionsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(FavouritePage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                        //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                        new FootballMasterDetailPageMenuItem { Id = 10, Title = "OnTrack +", IconSource="" , TargetType = typeof(AuthenticationPage), Group = "OnTrack" }
                    });

                    OnTrackHeight = 50 * OnTrackMenuItems.Count;
                }


                //AdminMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                //{
                //    new FootballMasterDetailPageMenuItem { Id = 0, Title = "Coaches", IconSource="" , TargetType = typeof(Admin.CoachPage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 1, Title = "Field", IconSource="" , TargetType = typeof(Admin.FieldPage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 2, Title = "Fixtures", IconSource="" , TargetType = typeof(Admin.FixturePage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leagues", IconSource="" , TargetType = typeof(Admin.LeaguePage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 4, Title = "Players", IconSource="" , TargetType = typeof(Admin.PlayerPage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 5, Title = "Teams", IconSource="" , TargetType = typeof(Admin.TeamPage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 6, Title = "Users", IconSource="" , TargetType = typeof(Admin.UserPage), Group = "Admin" },
                //    new FootballMasterDetailPageMenuItem { Id = 7, Title = "User Types", IconSource="" , TargetType = typeof(Admin.UserTypeEditPage), Group = "Admin" }
                //});

                //AdminHeight = 50 * AdminMenuItems.Count;




                //}
                //else
                //{
                //    OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                //    {
                //        new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(TablePage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 3, Title = "Competitions", IconSource="" , TargetType = typeof(CompetitionsPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 4, Title = "Transfers", IconSource="" , TargetType = typeof(TransferPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(FavouritePage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                //        //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                //        new FootballMasterDetailPageMenuItem { Id = 10, Title = "OnTrack Plus", IconSource="" , TargetType = typeof(AuthenticationPage), Group = "OnTrack" },
                //    });

                //    OnTrackHeight = 50 * OnTrackMenuItems.Count;
                //}
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
