using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class MyTicketPageViewModel : BaseViewModel
    {
        private ObservableCollection<Order> _orderCollection;
        private FixtureProduct _fixtureProduct;
        private PaymentAuthorize _paymentAuthorize;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        OrderService orderService;

        INavigation Navigation;

        public MyTicketPageViewModel(INavigation navigation)
        {
            orderService = new OrderService();
            OrderCollection = new ObservableCollection<Order>();

            GenerateSource();
        }

        public ObservableCollection<Order> OrderCollection
        {
            get { return _orderCollection; }
            set { this._orderCollection = value; }
        }

        public bool NoTickets
        {
            get { return _noTickets; }
            set { Set(ref _noTickets, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            try
            {
                NoTickets = false;
                IsActivityIndicatorVisible = true;

                var email = await SecureStorage.GetAsync("Email");
                var matchTickets = await orderService.GetMatchDayOrder(email                );
                OrderCollection = new ObservableCollection<Order>(matchTickets);

                if (OrderCollection.Count == 0)
                    NoTickets = true;

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
        }
    }
}
