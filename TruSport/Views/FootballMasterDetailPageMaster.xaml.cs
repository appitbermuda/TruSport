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
using TruSport.Views.Setting;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMasterDetailPageMaster : ContentPage
    {
        public SfListView OnTrackListView;


        public FootballMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new FootballMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        //async void SignOutTapped(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        var signout = await DisplayAlert("Sign Out","Are your sure you want to sign out?","Sign Out","Cancel");

        //        if (signout)
        //        {
        //            SignOutLabel.BackgroundColor = (Color)App.Current.Resources["primaryBarBlue"];

        //            SecureStorage.RemoveAll();

        //            App.IsLoggedIn = false;

        //            App.Current.MainPage = new FootballMasterDetailPage();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("SIGN OUT", "There was an issue logging you out. Please try again.", "Okay");
        //    }
        //    finally
        //    {
        //        SignOutLabel.BackgroundColor = Color.Transparent;
        //    }
        //}

        class FootballMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<FootballMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public bool SignOutVisible { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<FootballMasterDetailPageMenuItem> OnTrackMenuItems
            {
                set
                {
                    if (_onTrackMenuItems != value)
                    {
                        _onTrackMenuItems = value;
                        OnPropertyChanged("OnTrackMenuItems");
                    }
                }
                get { return _onTrackMenuItems; }
            }

            public FootballMasterDetailPageMasterViewModel()
            {
                SignOutVisible = false;

                GenerateMenu();
            }

            internal async void GenerateMenu()
            {
                
                OnTrackMenuItems = new ObservableCollection<FootballMasterDetailPageMenuItem>(new[]
                {
                    new FootballMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(FootballMasterDetailPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Football.FixturePage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Football.TeamPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(Football.TablePage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(Football.LeagueStatsPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(Football.CompetitionsPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Football.FavouritePage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(Tickets.TicketTabbedPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 9, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 8, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage), Group = "OnTrack" },
                    //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                    new FootballMasterDetailPageMenuItem { Id = 9, Title = "Sports", IconSource="" , TargetType = typeof(OnTrackPage), Group = "OnTrack" },
                    //new FootballMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(Tickets.TicketTabbedPage), Group = "OnTrack" }
                });
                
                OnTrackHeight = 50 * OnTrackMenuItems.Count;
                
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
