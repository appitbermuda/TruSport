using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Syncfusion.ListView.XForms;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Shop
{
    public class TicketListPageViewModel : BaseViewModel
    {
        private ObservableCollection<SportEvent> _sportEventCollection;
        private List<SportEvent> _ticketCollection;
        private ObservableCollection<object> _selectedTickets;
        private SportEvent _sportEvent;
        private bool _noTickets;
        private bool _isActivityIndicatorVisible;

        SportEventService sportEventService;
        InventoryService inventoryService;
        AdService adService;

        INavigation Navigation;

        public TicketListPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            sportEventService = new SportEventService();
            inventoryService = new InventoryService();
            adService = new AdService();
            SportEventCollection = new ObservableCollection<SportEvent>();
            TicketCollection = new List<SportEvent>();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
            //TicketSelectedCommand = new Command<object>(TicketSelected);
            CheckoutCommand = new Command(async () => await Checkout());

            MessagingCenter.Unsubscribe<TicketListPage, string>(this, "Refresh");
            MessagingCenter.Subscribe<TicketListPage>(this, "Refresh", async (obj) =>
            {
                GenerateSource();
            });
        }

        public Command CheckoutCommand { get; set; }
        public Command AdTappedCommand { get; }
        private Command<Object> ticketSelectedCommand;
        public Command<object> TicketSelectedCommand
        {
            get { return ticketSelectedCommand; }
            set { Set(ref ticketSelectedCommand, value); }
        }

        public List<SportEvent> TicketCollection
        {
            get { return _ticketCollection; }
            set { Set(ref _ticketCollection, value); }
        }

        public ObservableCollection<SportEvent> SportEventCollection
        {
            get { return _sportEventCollection; }
            set { Set(ref _sportEventCollection, value); }
        }

        public ObservableCollection<object> SelectedTickets
        {
            get { return this._selectedTickets; }
            set
            {
                SetProperty(ref _selectedTickets, value);
            }
        }

        public SportEvent SportEvent
        {
            get { return _sportEvent; }
            set { Set(ref _sportEvent, value); }
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

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        internal async void GenerateSource()
        {
            try
            {
                NoTickets = false;
                IsActivityIndicatorVisible = true;
                SelectedTickets = new ObservableCollection<object>();

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

                var eventTickets = await sportEventService.GetTickets();

                if (eventTickets != null)
                {
                    SportEventCollection = new ObservableCollection<SportEvent>(eventTickets);

                    if (SportEventCollection.Count == 0)
                        NoTickets = true;
                }
                else
                    NoTickets = true;
                
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

        //private async void TicketSelected(object obj)
        //{
        //    try
        //    {
        //        var listView = obj as SfListView;
        //        var sportEvent = listView.SelectedItems;

        //        if (sportEvent != null)
        //        {
        //            MessagingCenter.Subscribe<TicketPurchasePageViewModel>(this, "TicketListPage", async (objs) =>
        //            {
        //            //Purchase tickets saved to local
        //            //var creditCards = await App.Database.T(Customer.Email);
        //            GenerateSource();
        //            });

        //            string Token = await SecureStorage.GetAsync("Token");
        //            string email = await SecureStorage.GetAsync("Email");

        //            if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
        //            {
        //                bool guestCheckout = await App.Current.MainPage.DisplayAlert("Purchase Ticket", "Choose whether you would like to purchase tickets as a guest, or sign in to your account or create an account.", "Guest Purchase", "Sign In");

        //                if (guestCheckout)
        //                {
        //                    await Navigation.PushModalAsync(new TicketGuestPurchasePage(sportEvent));
        //                }
        //                else
        //                {
        //                    SportEvent = sportEvent;
        //                    MessagingCenter.Unsubscribe<LoginViewModel>(this, "OpenPurchasePage");
        //                    MessagingCenter.Subscribe<LoginViewModel>(this, "OpenPurchasePage", async (objs) =>
        //                    {
        //                        await Navigation.PushModalAsync(new TicketPurchasePage(SportEvent));
        //                    });

        //                    await Navigation.PushAsync(new SignInPage());
        //                }
        //            }
        //            else
        //            {
        //                var hasStock = await inventoryService.CheckEventTicketInventory(sportEvent.ID);

        //                if (hasStock)
        //                {

        //                    await Navigation.PushModalAsync(new TicketPurchasePage(sportEvent));
        //                }
        //                else
        //                {
        //                    GenerateSource();
        //                    await Application.Current.MainPage.DisplayAlert("Out of Stock", "Sorry, there are no more tickets left for purchase.", "OK");

        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        async Task Checkout()
        {
            try
            {
                List<SportEvent> sportEvents = new List<SportEvent>();
                foreach (var ticket in SelectedTickets)
                {
                    var ticketItem = ticket as SportEvent;
                    sportEvents.Add(ticketItem);
                }
                
                if (sportEvents != null && sportEvents.Count > 0)
                {
                    MessagingCenter.Subscribe<TicketPurchasePageViewModel>(this, "TicketListPage", async (objs) =>
                    {
                        //Purchase tickets saved to local
                        //var creditCards = await App.Database.T(Customer.Email);
                        GenerateSource();
                    });

                    string Token = await SecureStorage.GetAsync("Token");
                    string email = await SecureStorage.GetAsync("Email");

                    if (String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(email))
                    {
                        bool guestCheckout = await App.Current.MainPage.DisplayAlert("Purchase Ticket", "Choose whether you would like to purchase tickets as a guest, or sign in to your account or create an account.", "Guest Purchase", "Sign In");

                        if (guestCheckout)
                        {
                            await Navigation.PushAsync(new TicketPurchasePage(sportEvents, true));
                        }
                        else
                        {
                            TicketCollection = sportEvents;
                            MessagingCenter.Unsubscribe<LoginViewModel>(this, "OpenPurchasePage");
                            MessagingCenter.Subscribe<LoginViewModel>(this, "OpenPurchasePage", async (objs) =>
                            {
                                await Navigation.PushAsync(new TicketPurchasePage(TicketCollection));
                            });

                            await Navigation.PushAsync(new SignInPage());
                        }
                    }
                    else
                    {
                        bool hasStock = true;                        

                        
                        foreach(var sportEvent in sportEvents.Where(e => e.EventTickets.Any(d => d.Product.Age != "Spot #")))
                        {
                            hasStock = await inventoryService.CheckEventTicketInventory(sportEvent.ID);
                        }

                        if (hasStock)
                        {
                            await Navigation.PushAsync(new TicketPurchasePage(sportEvents));
                        }
                        else
                        {
                            GenerateSource();
                            await Application.Current.MainPage.DisplayAlert("Out of Stock", "Sorry, one or more tickets may no longer be available for purchase.", "OK");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
