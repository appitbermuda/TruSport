using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using TruSport.Views.Football;
using TruSport.Views.Setting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruSport.Views.Tennis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TennisMasterDetailPageMaster : ContentPage
    {
        public SfListView OnTrackListView;

        public TennisMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new TennisMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class TennisMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<TennisMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<TennisMasterDetailPageMenuItem> OnTrackMenuItems
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

            public TennisMasterDetailPageMasterViewModel()
            {
                OnTrackMenuItems = new ObservableCollection<TennisMasterDetailPageMenuItem>(new[]
                {
                    new TennisMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(TennisMasterDetailPage) },
                    //new TennisMasterDetailPageMenuItem { Id = 1, Title = "Fixtures", IconSource="" , TargetType = typeof(Bowling.FixturePage) },
                    //new TennisMasterDetailPageMenuItem { Id = 2, Title = "Teams", IconSource="" , TargetType = typeof(Bowling.TeamPage) },
                    new TennisMasterDetailPageMenuItem { Id = 3, Title = "Tournaments", IconSource="" , TargetType = typeof(Tennis.TournamentPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Rankings", IconSource="" , TargetType = typeof(Tennis.RankingPage) },
                    new TennisMasterDetailPageMenuItem { Id = 5, Title = "Favourites", IconSource="" , TargetType = typeof(Tennis.FavouritePage) },
                    new TennisMasterDetailPageMenuItem { Id = 9, Title = "Tickets", IconSource="" , TargetType = typeof(Tickets.TicketFlyoutPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "News", IconSource="" , TargetType = typeof(NewsPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Help", IconSource="" , TargetType = typeof(HelpPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage) },
                    new TennisMasterDetailPageMenuItem { Id = 4, Title = "Sports", IconSource="" , TargetType = typeof(OnTrackPage) },
                });
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
