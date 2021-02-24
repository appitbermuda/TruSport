using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Syncfusion.ListView.XForms;
using TruSport.Data;
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
        private ObservableCollection<AcceptTransfer> _transferRequestCollection;
        private ObservableCollection<MatchTicket> _matchTicketCollection;
        private ObservableCollection<CustomerOrder> _orderCollection;
        private Customer _customer;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        OrderService orderService;
        MatchTicketService matchTicketService;
        PushNotificationService pushNotificationService;
        AdService adService;

        INavigation Navigation;

        public MyTicketPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            matchTicketService = new MatchTicketService();
            orderService = new OrderService();
            adService = new AdService();
            pushNotificationService = new PushNotificationService();
            OrderCollection = new ObservableCollection<CustomerOrder>();
            MatchTicketCollection = new ObservableCollection<MatchTicket>();
            TransferRequestCollection = new ObservableCollection<AcceptTransfer>();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
            TicketSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(TicketSelected);
            AcceptTransferCommand = new Command<AcceptTransfer>(async (transfer) => await AcceptTransfer(transfer));
            RejectTransferCommand = new Command<AcceptTransfer>(async (transfer) => await RejectTransfer(transfer));

            MessagingCenter.Unsubscribe<ActiveTicketsPage, string>(this, "Refresh");
            MessagingCenter.Subscribe<ActiveTicketsPage>(this, "Refresh", async (obj) =>
            {
                GenerateSource();
            });
        }

        public Command AdTappedCommand { get; }
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ticketSelectedCommand;
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> TicketSelectedCommand
        {
            get { return ticketSelectedCommand; }
            set { Set(ref ticketSelectedCommand, value); }
        }

        private Command<AcceptTransfer> rejectTransferCommand;
        public Command<AcceptTransfer> RejectTransferCommand
        {
            get { return rejectTransferCommand; }
            set { Set(ref rejectTransferCommand, value); }
        }

        private Command<AcceptTransfer> acceptTransferCommand;
        public Command<AcceptTransfer> AcceptTransferCommand
        {
            get { return acceptTransferCommand; }
            set { Set(ref acceptTransferCommand, value); }
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

        public ObservableCollection<AcceptTransfer> TransferRequestCollection
        {
            get { return _transferRequestCollection; }
            set { Set(ref _transferRequestCollection, value); }
        }

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        public bool NoTickets
        {
            get { return _noTickets; }
            set { Set(ref _noTickets, value); }
        }

        public Customer Customer
        {
            get { return _customer; }
            set { Set(ref _customer, value); }
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
                    await Task.Run(async () =>
                    {
                        var ads = await adService.GetAds();

                        if (ads != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Ad = ads.Any(e => e.Sport == Constants.Bowling) ? ads.FirstOrDefault(e => e.Sport == Constants.Bowling) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var email = await SecureStorage.GetAsync("Email");
                    Customer = await App.Database.GetCustomerByIDAsync(email);
                    var matchTickets = await matchTicketService.GetMatchTickets();

                    var transferRequests = await matchTicketService.GetTransferRequests();
                    if(transferRequests != null)
                    {
                        TransferRequestCollection = new ObservableCollection<AcceptTransfer>(transferRequests.Where(e => e.TransferCustomer.Email == email));
                    }

                    if (matchTickets != null)
                    {
                        matchTickets.ForEach(e => e.CustomerTicket = JsonConvert.SerializeObject(e.CustomerMatchTicket));
                        
                        MatchTicketCollection = new ObservableCollection<MatchTicket>(matchTickets);
                    }

                    if (MatchTicketCollection.Count == 0 && TransferRequestCollection.Count == 0)
                        NoTickets = true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Not Network", "Please check your network connection and come back.", "Okay");
                }
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

        private async void TicketSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                var acceptTransfer = e.ItemData as MatchTicket;

                if (!acceptTransfer.Validated)
                {

                    bool transferTicket = await App.Current.MainPage.DisplayAlert("Transfer Ticket", "Would you like to transfer this match ticket?", "Yes", "Cancel");

                    if (transferTicket)
                    {
                        MessagingCenter.Unsubscribe<TransferTicketPageViewModel,bool>(this, "TicketTransferred");
                        MessagingCenter.Subscribe<TransferTicketPageViewModel,bool>(this, "TicketTransferred", async (objs, transferred) =>
                        {
                            //Purchase tickets saved to local
                            //var creditCards = await App.Database.T(Customer.Email);
                            
                            if(transferred)
                                GenerateSource();
                        });

                        await Navigation.PushModalAsync(new TransferTicketPage(acceptTransfer));
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketSelected");

            }
        }

        private async void AdTapped()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await adService.Impressions(Ad.ID);
                });

                await Launcher.OpenAsync(new Uri(Ad.URL));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad Tapped");
            }
        }

        private async Task AcceptTransfer(AcceptTransfer acceptTransfer)
        {
            
            //var acceptTransfer = obj as AcceptTransfer;
            bool transferTicket = await App.Current.MainPage.DisplayAlert("Accept Ticket Transfer", "Are you sure you want to accept this ticket transfer?", "Yes", "Cancel");
            
            if (transferTicket)
            {
                acceptTransfer.Accept = true;
                string accept = await matchTicketService.AcceptTransfer(acceptTransfer);

                try
                {
                    await pushNotificationService.Send(new NotificationRequest
                    {
                        Text = Customer.FirstName + "has accepted your match ticket transfer.",
                        Silent = false,
                        Tags = new string[] { acceptTransfer.Customer.Email }
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Transfer Notification");
                }

                await App.Current.MainPage.DisplayAlert("Ticket Transfer", accept, "Okay");

                GenerateSource();
            }
        }

        private async Task RejectTransfer(AcceptTransfer acceptTransfer)
        {

            //var acceptTransfer = obj as AcceptTransfer;
            
            bool transferTicket = await App.Current.MainPage.DisplayAlert("Reject Ticket Transfer", "Are you sure you want to reject this ticket transfer?", "Yes", "Cancel");
            

            if (transferTicket)
            {
                acceptTransfer.Accept = false;
                string accept = await matchTicketService.AcceptTransfer(acceptTransfer);

                try
                {
                    await pushNotificationService.Send(new NotificationRequest
                    {
                        Text = Customer.FirstName + "has rejected your match ticket transfer.",
                        Silent = false,
                        Tags = new string[] { acceptTransfer.Customer.Email }
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Transfer Notification");
                }

                await App.Current.MainPage.DisplayAlert("Ticket Transfer", accept, "Okay");

                GenerateSource();
            }
        }
    }
}
