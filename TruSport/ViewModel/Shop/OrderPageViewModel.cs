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
    public class OrderPageViewModel : BaseViewModel
    {
        private ObservableCollection<Order> _orderCollection;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        OrderService orderService;

        INavigation Navigation;

        public OrderPageViewModel(INavigation navigation)
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

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var email = await SecureStorage.GetAsync("Email");
                    var matchTickets = await orderService.GetOrderHistory(email);
                    OrderCollection = new ObservableCollection<Order>(matchTickets);

                    if (OrderCollection.Count == 0)
                        NoTickets = true;
                }
                else
                {
                    var email = await SecureStorage.GetAsync("Email");
                    var matchTickets = await App.Database.GetOrderHistory(email);
                    OrderCollection = new ObservableCollection<Order>(matchTickets);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Orders");
            }

            IsActivityIndicatorVisible = false;
        }
    }
}
