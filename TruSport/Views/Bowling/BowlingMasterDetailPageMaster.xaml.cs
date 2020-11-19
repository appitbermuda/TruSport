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

namespace TruSport.Views.Bowling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BowlingMasterDetailPageMaster : ContentPage
    {
        public SfListView OnTrackListView;

        public BowlingMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new BowlingMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class BowlingMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<BowlingMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<BowlingMasterDetailPageMenuItem> OnTrackMenuItems
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

            public BowlingMasterDetailPageMasterViewModel()
            {
                OnTrackMenuItems = new ObservableCollection<BowlingMasterDetailPageMenuItem>(new[]
                {
                    new BowlingMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(BowlingMasterDetailPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 1, Title = "Fixtures", IconSource="" , TargetType = typeof(Bowling.FixturePage) },
                    new BowlingMasterDetailPageMenuItem { Id = 2, Title = "Teams", IconSource="" , TargetType = typeof(Bowling.TeamPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 3, Title = "Standings", IconSource="" , TargetType = typeof(Bowling.TablePage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Leaderboard", IconSource="" , TargetType = typeof(Bowling.LeagueStatsPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "News", IconSource="" , TargetType = typeof(NewsPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Help", IconSource="" , TargetType = typeof(HelpPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage) },
                    new BowlingMasterDetailPageMenuItem { Id = 4, Title = "Sports", IconSource="" , TargetType = typeof(OnTrackPage) },
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
