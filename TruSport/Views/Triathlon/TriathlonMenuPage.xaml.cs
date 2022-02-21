using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Syncfusion.ListView.XForms;
using TruSport.Views.Setting;
using Xamarin.Forms;

namespace TruSport.Views.Triathlon
{
    public partial class TriathlonMenuPage : ContentPage
    {
        public SfListView OnTrackListView;

        public TriathlonMenuPage()
        {
            InitializeComponent();

            BindingContext = new TriathlonMenuPageViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }
        
        class TriathlonMenuPageViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<TriathlonMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public bool SignOutVisible { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<TriathlonMasterDetailPageMenuItem> OnTrackMenuItems
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

            public TriathlonMenuPageViewModel()
            {
                SignOutVisible = false;

                GenerateMenu();
            }

            internal async void GenerateMenu()
            {

                OnTrackMenuItems = new ObservableCollection<TriathlonMasterDetailPageMenuItem>(new[]
                {
                    new TriathlonMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(TriathlonMainPage), Group = "OnTrack" },
                    //new TriathlonMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Triathlon.FixturePage), Group = "OnTrack" },
                    //new TriathlonMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource=" " , TargetType = typeof(Triathlon.TeamPage), Group = "OnTrack" },
                    //new TriathlonMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(Triathlon.TablePage), Group = "OnTrack" },
                    //new TriathlonMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(Triathlon.LeagueStatsPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(Triathlon.CompetitionsPage), Group = "OnTrack" },
                    //new TriathlonMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Triathlon.FavouritePage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(MainPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 9, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 8, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                    new TriathlonMasterDetailPageMenuItem { Id = 9, Title = "Switch Sports", IconSource="" , TargetType = typeof(OnTrackPage), Group = "OnTrack" }
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
