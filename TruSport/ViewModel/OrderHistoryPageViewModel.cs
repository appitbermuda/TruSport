using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class OrderHistoryPageViewModel : BaseViewModel
    {
        private ObservableCollection<Order> _orderCollection;
        private bool _isOrderActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;

        INavigation Navigation;
        OrderService orderService;

        public OrderHistoryPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            orderService = new OrderService();
            OrderCollection = new ObservableCollection<Order>();

            GenerateSource();
        }

        public ObservableCollection<Order> OrderCollection
        {
            get { return _orderCollection; }
            set { Set(ref _orderCollection, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {

                    var email = await SecureStorage.GetAsync("Email");

                    var orderHistory = await orderService.GetOrderHistory(email);
                    if (orderHistory != null)
                        OrderCollection = new ObservableCollection<Order>(orderHistory);

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order History");
            }

            IsActivityIndicatorVisible = false;
        }
    }
}
