using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Syncfusion.ListView.XForms;
using TruSport.Views.Setting;
using Xamarin.Forms;

namespace TruSport.Views.Basketball
{
    public partial class BasketballMenuPage : ContentPage
    {
        public SfListView OnTrackListView;

        public BasketballMenuPage()
        {
            InitializeComponent();

            BindingContext = new BasketallMenuPageViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class BasketallMenuPageViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<BasketballMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public bool SignOutVisible { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<BasketballMasterDetailPageMenuItem> OnTrackMenuItems
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

            public BasketallMenuPageViewModel()
            {
                SignOutVisible = false;

                GenerateMenu();
            }

            internal async void GenerateMenu()
            {

                OnTrackMenuItems = new ObservableCollection<BasketballMasterDetailPageMenuItem>(new[]
                {
                    new BasketballMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(BasketballMainPage), Group = "OnTrack" },
                    //new BasketballMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Basketball.FixturePage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource="" , TargetType = typeof(Basketball.TeamPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(Basketball.TablePage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(Basketball.LeagueStatsPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(Basketball.CompetitionsPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Basketball.FavouritePage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(MainPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 9, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 8, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                    new BasketballMasterDetailPageMenuItem { Id = 9, Title = "Switch Sports", IconSource="" , TargetType = typeof(OnTrackPage), Group = "OnTrack" }
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
