using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Syncfusion.ListView.XForms;
using TruSport.Views.Setting;
using Xamarin.Forms;

namespace TruSport.Views.Golf
{
    public partial class GolfMenuPage : ContentPage
    {
        public SfListView OnTrackListView;

        public GolfMenuPage()
        {
            InitializeComponent();

            BindingContext = new GolfMenuPageViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class GolfMenuPageViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<GolfMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public bool SignOutVisible { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<GolfMasterDetailPageMenuItem> OnTrackMenuItems
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

            public GolfMenuPageViewModel()
            {
                SignOutVisible = false;

                GenerateMenu();
            }

            internal async void GenerateMenu()
            {
                OnTrackMenuItems = new ObservableCollection<GolfMasterDetailPageMenuItem>(new[]
                {
                    new GolfMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(GolfMainPage), Group = "OnTrack" },
                    //new GolfMasterDetailPageMenuItem { Id = 0, Title = "Fixtures", IconSource="" , TargetType = typeof(Golf.FixturePage), Group = "OnTrack" },
                    //new GolfMasterDetailPageMenuItem { Id = 1, Title = "Teams", IconSource=" " , TargetType = typeof(Golf.TeamPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 2, Title = "Standings", IconSource="" , TargetType = typeof(Golf.StandingsPage), Group = "OnTrack" },
                    //new GolfMasterDetailPageMenuItem { Id = 3, Title = "Leaderboard", IconSource="" , TargetType = typeof(Golf.TablePage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 4, Title = "Competitions", IconSource="" , TargetType = typeof(Golf.CompetitionsPage), Group = "OnTrack" },
                    //new GolfMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Golf.FavouritePage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(MainPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 9, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 6, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 7, Title = "News", IconSource="" , TargetType = typeof(NewsPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 8, Title = "Help", IconSource="" , TargetType = typeof(HelpPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 8, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage), Group = "OnTrack" },
                    new GolfMasterDetailPageMenuItem { Id = 9, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage), Group = "OnTrack" },
                    //new GolfMasterDetailPageMenuItem { Id = 9, Title = "Switch Sports", IconSource="" , TargetType = typeof(OnTrackPage), Group = "OnTrack" }
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

        async void AllSportsButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await App.Database.ClearDefaultSport();
                App.Current.MainPage = new OnTrackPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
