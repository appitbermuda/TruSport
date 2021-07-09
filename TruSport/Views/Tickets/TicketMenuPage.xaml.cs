using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Syncfusion.ListView.XForms;
using TruSport.Views.Football;
using TruSport.Views.Setting;
using Xamarin.Forms;

namespace TruSport.Views.Tickets
{
    public partial class TicketMenuPage : ContentPage
    {
        public SfListView OnTrackListView;

        public TicketMenuPage()
        {
            InitializeComponent();

            BindingContext = new TicketMasterDetailPageMasterViewModel();
            OnTrackListView = OnTrackMenuItemsListView;
        }

        class TicketMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<TicketMasterDetailPageMenuItem> _onTrackMenuItems { get; set; }
            public double OnTrackHeight { get; set; }

            public ObservableCollection<TicketMasterDetailPageMenuItem> OnTrackMenuItems
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

            public TicketMasterDetailPageMasterViewModel()
            {
                OnTrackMenuItems = new ObservableCollection<TicketMasterDetailPageMenuItem>(new[]
                {
                    new TicketMasterDetailPageMenuItem { Id = 0, Title = "Home", IconSource="" , TargetType = typeof(TicketListPage) },
                    new TicketMasterDetailPageMenuItem { Id = 3, Title = "My Account", IconSource="" , TargetType = typeof(Tickets.AccountPage) },
                    new TicketMasterDetailPageMenuItem { Id = 3, Title = "My Tickets", IconSource="" , TargetType = typeof(Tickets.MyTicketsPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Order History", IconSource="" , TargetType = typeof(Tickets.OrderHistoryPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Shop Merchandise", IconSource="" , TargetType = typeof(ShopPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "What's Happening?", IconSource="" , TargetType = typeof(FlyersPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "News", IconSource="" , TargetType = typeof(NewsPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Help", IconSource="" , TargetType = typeof(HelpPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Settings", IconSource="" , TargetType = typeof(SettingPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Contact Us", IconSource="" , TargetType = typeof(ContactUsPage) },
                    new TicketMasterDetailPageMenuItem { Id = 4, Title = "Sports", IconSource="" , TargetType = typeof(OnTrackPage) },
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
