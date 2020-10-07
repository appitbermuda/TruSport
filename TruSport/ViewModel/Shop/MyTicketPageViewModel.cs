using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Newtonsoft.Json;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class MyTicketPageViewModel : BaseViewModel
    {
        private ObservableCollection<MatchTicket> _matchTicketCollection;
        private ObservableCollection<CustomerOrder> _orderCollection;
        private FixtureProduct _fixtureProduct;
        private PaymentAuthorize _paymentAuthorize;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        OrderService orderService;
        MatchTicketService matchTicketService;

        INavigation Navigation;

        public MyTicketPageViewModel(INavigation navigation)
        {
            matchTicketService = new MatchTicketService();
            orderService = new OrderService();
            OrderCollection = new ObservableCollection<CustomerOrder>();
            MatchTicketCollection = new ObservableCollection<MatchTicket>();

            GenerateSource();

            MessagingCenter.Unsubscribe<ActiveTicketsPage, string>(this, "Refresh");
            MessagingCenter.Subscribe<ActiveTicketsPage>(this, "Refresh", async (obj) =>
            {
                GenerateSource();
            });
        }

        public ObservableCollection<MatchTicket> MatchTicketCollection
        {
            get { return _matchTicketCollection; }
            set { Set(ref _matchTicketCollection, value); }
        }

        public ObservableCollection<CustomerOrder> OrderCollection
        {
            get { return _orderCollection; }
            set { Set(ref _orderCollection, value); }
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
                    var matchTickets = await matchTicketService.GetMatchTickets();
                    //var matchTickets = await orderService.GetMatchDayOrder(email);

                    if (matchTickets != null)
                    {
                        matchTickets.ForEach(e => e.CustomerTicket = JsonConvert.SerializeObject(e.CustomerMatchTicket));
                        //OrderCollection = new ObservableCollection<CustomerOrder>(matchTickets);
                        MatchTicketCollection = new ObservableCollection<MatchTicket>(matchTickets);
                    }

                    if (MatchTicketCollection.Count == 0)
                        NoTickets = true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Not Network", "Please check your network connection and come back.", "Okay");
                }
                //else
                //{
                //    var email = await SecureStorage.GetAsync("Email");
                //    var matchTickets = await App.Database.GetMatchDayOrder(email);
                //    var matchTickets = await App.Database.GetMatchTickets();

                //    if (matchTickets != null)
                //    {
                //        matchTickets.ForEach(e => e.CustomerTicket = JsonConvert.SerializeObject(e));
                //        //OrderCollection = new ObservableCollection<CustomerOrder>(matchTickets);
                //        MatchTicketCollection = new ObservableCollection<MatchTicket>(matchTickets);
                //    }

                //    //if (OrderCollection.Count == 0)
                //    //    NoTickets = true;
                //    if (MatchTicketCollection.Count == 0)
                //        NoTickets = true;
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Tickets");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }
    }
}
