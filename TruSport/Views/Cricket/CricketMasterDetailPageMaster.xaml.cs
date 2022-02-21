using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TruSport.Views.Cricket;
using TruSport.Views.Football;
using TruSport.Views.Setting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views.Cricket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CricketMasterDetailPageMaster : ContentPage
    {
        public SfListView OnTrackListView;

        public CricketMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new CricketMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class CricketMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<CricketMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public double OnTrackHeight { get; set; }
            public ObservableCollection<CricketMasterDetailPageMenuItem> OnTrackMenuItems
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

            public CricketMasterDetailPageMasterViewModel()
            {
                OnTrackMenuItems = new ObservableCollection<CricketMasterDetailPageMenuItem>(new[]
                {
                    new CricketMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(CricketMasterDetailPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Cricket.FixturePage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Cricket.TeamPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(Cricket.TablePage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(Cricket.LeagueStatsPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(Cricket.CompetitionsPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Cricket.FavouritePage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(MainPage), Group = "OnTrack" },
                //new CricketMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(Tickets.TicketFlyoutPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 9, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 8, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage), Group = "OnTrack" },
                //new CricketMasterDetailPageMenuItem { Id = 9, Title = "Privacy Policy", IconSource="" , TargetType = typeof(PrivacyPolicyPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                new CricketMasterDetailPageMenuItem { Id = 9, Title = "Sports", IconSource="" , TargetType = typeof(OnTrackPage), Group = "OnTrack" },
                //new CricketMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(Tickets.TicketTabbedPage), Group = "OnTrack" }
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
